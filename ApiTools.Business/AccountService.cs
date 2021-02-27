using ApiTools.Business.Contracts;
using ApiTools.Data;
using ApiTools.Domain;
using ApiTools.Domain.Data;
using ApiTools.Domain.Exceptions;
using ApiTools.Domain.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApiTools.Business
{
    internal class AccountService : IAccountService
    {
        private readonly AppSettings appSettings;
        private readonly ApiDbContext dbContext;
        private readonly ILogger<AccountService> logger;
        private readonly IPasswordHasher<Account> passwordHasher;

        public AccountService(IOptions<AppSettings> appOptions, ILogger<AccountService> logger,
            ApiDbContext dbContext, IPasswordHasher<Account> passwordHasher)
        {
            appSettings = appOptions?.Value ?? throw new ArgumentException("No AppSettings options found!");
            this.dbContext = dbContext;
            this.logger = logger;
            this.passwordHasher = passwordHasher;
        }

        public async Task<AccountEmail> CreateEmailAsync(string validatedEmail, EmailType emailType = EmailType.Primary, CancellationToken cancellationToken = default)
        {
            var accountEmail = await dbContext.Emails.FirstOrDefaultAsync(e => e.Email == validatedEmail, cancellationToken)
                .ConfigureAwait(false);
            if (accountEmail != null)
            {
                if (accountEmail.AccountId != null || accountEmail.Type.HasFlag(EmailType.Banned))
                {
                    throw new ArgumentException("The provided email is already used.");
                }
                else
                {
                    accountEmail.Type = emailType;
                    accountEmail.Verified = false;

                    return accountEmail;
                }
            }


            return new AccountEmail
            {
                Email = validatedEmail,
                Type = emailType,
                Verified = false
            };
        }

        public async Task<AccountInvite> CreateNewInviteAsync(ulong inviterId, DateTime? utcExpire = null, CancellationToken cancellationToken = default)
        {
            AccountInvite invite = new AccountInvite
            {
                InviterId = inviterId,
                Invited = DateTime.UtcNow,
                Expire = utcExpire ?? DateTime.UtcNow.Add(appSettings.Security.Invite.Lifetime),
            };

            try
            {
                dbContext.Invites.Add(invite);
                await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (DbUpdateException ex)
            {
                logger.LogError(ex, "Couldn't create invite token due to a database exception.");
                throw;
            }

            logger.LogInformation("Account #{Account} created an invite token: {Token}", invite.InviterId, invite.Token);
            return invite;
        }


        public Task<Account> GetAccountAsync(ulong accountId, CancellationToken cancellationToken = default)
            => dbContext.Users
                .Include(a => a.ProfileEmail)
                .FirstOrDefaultAsync(a => a.Id == accountId, cancellationToken);

        public Task<Account> GetAccountAsync(string usernameOrEmail, CancellationToken cancellationToken = default)
            => dbContext.Users
                .Include(a => a.ProfileEmail)
                .FirstOrDefaultAsync(a => a.NormalizedUserName == usernameOrEmail.ToUpper() || a.Email == usernameOrEmail.ToLower(), cancellationToken);

        public Task<AccountInvite> GetInviteTokenAsync(Guid tokenId, CancellationToken cancellationToken = default)
            => dbContext.Invites
                .FirstOrDefaultAsync(i => i.Token == tokenId, cancellationToken);

        public Task<AccountInvite> GetInviteTokenAsync(Guid tokenId, ulong accountId, CancellationToken cancellationToken = default)
            => dbContext.Invites
                .FirstOrDefaultAsync(i => i.Token == tokenId
                    && (i.AcceptorId == accountId || i.InviterId == accountId),
                    cancellationToken);

        public async Task<IEnumerable<AccountInvite>> GetInviteTokensAsync(ulong inviterId, CancellationToken cancellationToken = default)
        {
            IEnumerable<AccountInvite> invites = await dbContext.Invites.Where(i => i.InviterId == inviterId)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            return invites;
        }

        public async Task<IReadOnlyList<BadField>> RegisterAccountAsync(RegistrationRequest request, CancellationToken cancellationToken = default)
        {
            List<BadField> badFields = new List<BadField>(request.Validate(appSettings.Limits.Account));

            if (badFields.Count < 1)
            {
                Account account = new Account
                {
                    RegisteredDate = DateTime.UtcNow,
                    UserName = request.Username,
                    FirstName = request.FirstName,
                    LastName = request.LastName
                };

                if (badFields.Count < 1)
                {
                    try
                    {
                        AccountEmail primaryEmail = await CreateEmailAsync(request.Email, EmailType.Primary, cancellationToken)
                                .ConfigureAwait(false);
                        account.PrimaryEmail = primaryEmail;
                    }
                    catch (ArgumentException)
                    {
                        badFields.Add(new BadField("email", BadField.AlreadyExists));
                    }

                    if (badFields.Count < 1)
                    {
                        account.NormalizedUserName = account.UserName.ToUpper();
                        await UpdatePasswordAsync(account, request.Password, cancellationToken)
                            .ConfigureAwait(false);

                        try
                        {
                            dbContext.Add(account);
                            await dbContext.SaveChangesAsync(cancellationToken)
                                .ConfigureAwait(false);
                            logger.LogInformation("A new account was created #{AccountId} using username ${Username}", account.Id, account.UserName);
                        }
                        catch (DbUpdateException ex)
                        {
                            logger.LogError(ex, "An error occurred adding a saving a newly created account.");
                            throw;
                        }
                    }
                }
            }
            return badFields;
        }
        #region UpdatePasswordAsync
        public async Task UpdatePasswordAsync(ulong accountId, string newValidatedPassword, CancellationToken cancellationToken = default)
        {
            Account account = await GetAccountAsync(accountId, cancellationToken)
                .ConfigureAwait(false);

            if (account == null)
            {
                throw new ApiNotFoundException("Couldn't find the account with the specified Account Id", false);
            }


            await UpdatePasswordAsync(account, newValidatedPassword, cancellationToken)
                .ConfigureAwait(false);
        }
        public Task UpdatePasswordAsync(Account account, string newValidatedPassword, CancellationToken cancellationToken = default)
        {
            return Task.Run(() =>
            {
                account.PasswordHash = passwordHasher.HashPassword(account, newValidatedPassword);
                logger.LogInformation("Updated password for account #{AccountId}", account.Id);
            }, cancellationToken);
        }
        #endregion
        #region VerifyPasswordAsync
        public async Task<bool> VerifyPasswordAsync(ulong accountId, string password, CancellationToken cancellationToken = default)
        {
            Account account = await GetAccountAsync(accountId, cancellationToken)
                .ConfigureAwait(false);

            if (account == null)
            {
                throw new ApiNotFoundException("Couldn't find the account with the specified Account Id", false);
            }

            return await VerifyPasswordAsync(account, password, cancellationToken)
                .ConfigureAwait(false);
        }
        public Task<bool> VerifyPasswordAsync(Account account, string password, CancellationToken cancellationToken = default)
        {
            return Task.Run(async () =>
            {
                PasswordVerificationResult passwordResult = passwordHasher.VerifyHashedPassword(account, account.PasswordHash, password);
                switch (passwordResult)
                {
                    case PasswordVerificationResult.SuccessRehashNeeded:
                        logger.LogInformation("A password rehash will run for #{AccountId}", account.Id);
                        await UpdatePasswordAsync(account, password, cancellationToken)
                            .ConfigureAwait(false);
                        return true;
                    case PasswordVerificationResult.Success:
                        return true;
                }

                return false;
            }, cancellationToken);
        }
        #endregion
    }
}
