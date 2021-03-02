using ApiTools.Business.Contracts;
using ApiTools.Data;
using ApiTools.Domain;
using ApiTools.Domain.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Wangkanai.Detection.Models;

namespace ApiTools.Business
{
    internal class AuthenticationService : IAuthenticationService
    {
        private readonly ApiDbContext dbContext;

        private readonly IAccountService accountService;
        private readonly ILogger<AuthenticationService> logger;
        private readonly IOptions<AppSettings> appOptions;
        private readonly ISmtpService smtpService;
        private readonly ITokenService tokenService;

        public AuthenticationService(ILogger<AuthenticationService> logger, IOptions<AppSettings> appOptions,
            IAccountService accountService, ISmtpService smtpService, ApiDbContext dbContext, ITokenService tokenService)
        {
            this.dbContext = dbContext;

            this.accountService = accountService;
            this.logger = logger;
            this.appOptions = appOptions;
            this.smtpService = smtpService;
            this.tokenService = tokenService;
        }

        public async Task<AccountSession> AuthenticateAsync(string usernameOrEmail, string password,
            Browser browser, Platform platform, byte[] ipAddress, CancellationToken cancellationToken = default)
        {
            if (ipAddress.Length != 4 || ipAddress.Length != 16)
            {
                throw new ArgumentException("Provided IP Address argument is an invalid length.");
            }

            Account account = await accountService.GetAccountAsync(usernameOrEmail, cancellationToken);

            var verifyResult = await accountService.VerifyPasswordAsync(account, password, cancellationToken);
            if (verifyResult)
            {
                AccountSession session = new AccountSession
                {
                    Account = account,
                    Browser = (byte)browser,
                    Platform = (byte)platform,
                    Ip = ipAddress,
                    Issued = DateTime.UtcNow,
                    Expires = DateTime.UtcNow.Add(appOptions.Value.Security.Session.Expires)
                };

                try
                {
                    dbContext.Add(session);
                    await dbContext.SaveChangesAsync()
                        .ConfigureAwait(false);

                    return session;
                }
                catch (DbUpdateException ex)
                {
                    logger.LogError(ex, "Couldn't generate account session due to a database exception.");
                    throw;
                }
            }

            return null;
        }

        public async Task<string> GetResetPasswordLinkAsync(ulong accountId, CancellationToken cancellationToken = default)
        {
            Account account = await accountService.GetAccountAsync(accountId, cancellationToken)
                .ConfigureAwait(false);

            return await GetResetPasswordLinkAsync(account, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<string> GetResetPasswordLinkAsync(Account account, CancellationToken cancellationToken = default)
        {
            // Setup claims
            Claim[] claims = new Claim[]
            {
                new Claim(TokenClaimNames.AccountId, $"{account.Id}"),
                new Claim(TokenClaimNames.Username, account.UserName),
                new Claim(TokenClaimNames.ConcurrencyStamp, account.ConcurrencyStamp)
            };

            // Generate token
            string token = await tokenService.CreateTokenAsync(claims, null);
            return $"{appOptions.Value.Endpoints.ResetPassword}{token}";
        }

        public async Task<string> GetSessionToken(AccountSession session, CancellationToken cancellationToken = default)
        {
            if (session.Account == null)
            {
                session.Account = await accountService.GetAccountAsync(session.AccountId.Value, cancellationToken)
                    .ConfigureAwait(false);
            }

            Claim[] claims = new Claim[]
            {
                new Claim(TokenClaimNames.AccountId, $"{session.Account.Id}"),
                new Claim(TokenClaimNames.Username, session.Account.UserName),
                new Claim(TokenClaimNames.Role, $"{session.Account.RoleId}"),
                new Claim(TokenClaimNames.Token, session.Id.ToString("N")),
            };

            return await tokenService.CreateTokenAsync(claims);
        }

        public async Task<string> GetSessionToken(Guid sessionId, CancellationToken cancellationToken = default)
        {
            AccountSession session = await dbContext.Sessions.Include(x => x.Account)
                .FirstOrDefaultAsync(x => x.Id == sessionId);

            return await GetSessionToken(session, cancellationToken);
        }

        public Task LogoutAllSessions(ulong accountId, CancellationToken cancellationToken = default)
            => dbContext.Sessions.Where(x => x.AccountId == accountId)
                .ForEachAsync(x => x.Expires = DateTime.UtcNow);

        public Task LogoutAllSessions(Account account, CancellationToken cancellationToken = default)
            => LogoutAllSessions(account.Id, cancellationToken);

        public Task LogoutSession(Guid sessionId, CancellationToken cancellationToken = default)
            => dbContext.Sessions.Where(x => x.Id == sessionId)
                .ForEachAsync(x => x.Expires = DateTime.UtcNow);

        public Task LogoutSession(AccountSession session, CancellationToken cancellationToken = default)
            => LogoutSession(session.Id, cancellationToken);

        public async Task ResetPasswordAsync(ulong accountId, CancellationToken cancellationToken = default)
        {
            Account account = await accountService.GetAccountAsync(accountId, cancellationToken)
                .ConfigureAwait(false);

            _ = ResetPasswordAsync(account, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task ResetPasswordAsync(Account account, CancellationToken cancellationToken = default)
        {
            string templatePath = appOptions.Value.Templates.ResetPassword;
            Dictionary<string, string> replacements = new Dictionary<string, string>
            {
                { "Username", account.UserName },
                { "FirstName", account.FirstName },
                { "LastName", account.LastName },
                { "AccountId", $"{account.Id}" }
            };

            // Generate the recovery link
            replacements.Add("ResetEndpoint",
                await GetResetPasswordLinkAsync(account, cancellationToken)
                    .ConfigureAwait(false));

            var htmlBody = await smtpService.FetchHtmlBodyAsync(templatePath, replacements, cancellationToken)
                .ConfigureAwait(false);

            // Generate email
            MimeMessage mimeMessage = new MimeMessage
            {
                Body = new TextPart("html", htmlBody),
                Subject = "Password reset request"
            };

            mimeMessage.To.Add(new MailboxAddress(account.FullName, account.Email));

            _ = smtpService.SendAsync(mimeMessage, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
