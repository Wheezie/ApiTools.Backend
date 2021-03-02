using ApiTools.Domain;
using ApiTools.Domain.Data;
using ApiTools.Domain.Requests;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ApiTools.Business.Contracts
{
    public interface IAccountService
    {
        /// <summary>
        /// Generate a new account email
        /// </summary>
        /// <remarks>
        /// NOTE: This will not save/update to the database, it will reuse the existing db entry but not update it.
        /// </remarks>
        /// <param name="validatedEmail">Email to assing</param>
        /// <param name="emailType">The type of the address</param>
        /// <param name="cancellationToken">Task cancellation token</param>
        /// <returns>A successfully generated email</returns>
        /// <exception cref="ArgumentException">Email is already used or invalid.</exception>
        Task<AccountEmail> CreateEmailAsync(string validatedEmail, EmailType emailType = EmailType.Primary, CancellationToken cancellationToken = default);
        /// <summary>
        /// Create a new invite token.
        /// </summary>
        /// <param name="inviterId">Inviter account identifier</param>
        /// <param name="utcExpire">UTC token expire datetime, defaults to the config setting</param>
        /// <param name="cancellationToken">Task cancellation token</param>
        /// <returns>The newly created invite token, null if failed</returns>
        Task<AccountInvite> CreateNewInviteAsync(ulong inviterId, DateTime? utcExpire = null, CancellationToken cancellationToken = default);
        /// <summary>
        /// Retrieve a specific invite token.
        /// </summary>
        /// <param name="tokenId">Token identifier</param>
        /// <param name="cancellationToken">Task cancellation token</param>
        /// <returns>The requested token, null if not found</returns>
        Task<AccountInvite> GetInviteTokenAsync(Guid tokenId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Retrieve a specific invite token.
        /// </summary>
        /// <param name="tokenId">Token identifier</param>
        /// <param name="accountId">Inviter or accepter account identifier.</param>
        /// <param name="cancellationToken">Task cancellation token</param>
        /// <returns>The requested token, null if not found</returns>
        Task<AccountInvite> GetInviteTokenAsync(Guid tokenId, ulong accountId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Retrieve all the invite tokens.
        /// </summary>
        /// <param name="inviterId">Inviter account identifier</param>
        /// <returns>All the created tokens</returns>
        Task<IEnumerable<AccountInvite>> GetInviteTokensAsync(ulong inviterId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Retrieve an account based on the username or email identifier.
        /// </summary>
        /// <param name="usernameOrEmail">Username or primary email as identifier</param>
        /// <param name="cancellationToken">Task cancellation token</param>
        Task<Account> GetAccountAsync(string usernameOrEmail, CancellationToken cancellationToken = default);
        /// <summary>
        /// Retrieve an account based on the account identifier.
        /// </summary>
        /// <param name="accountId">Account identifier</param>
        /// <param name="cancellationToken">Task cancellation token</param>
        Task<Account> GetAccountAsync(ulong accountId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Register an account using the provided credentials.
        /// </summary>
        /// <param name="request">Creation parameters (will be validated)</param>
        /// <param name="cancellationToken">Task cancellation token</param>
        /// <returns>A list of errors, empty if succeeded</returns>
        Task<IReadOnlyList<BadField>> RegisterAccountAsync(RegistrationRequest request, CancellationToken cancellationToken = default);
        /// <summary>
        /// Save an account to the persistent storage.
        /// </summary>
        /// <param name="account">Account to store</param>
        /// <param name="cancellationToken">Task cancellation token</param>
        Task UpdateAccount(Account account, CancellationToken cancellationToken = default);
        /// <summary>
        /// Update an account password.
        /// </summary>
        /// <remarks>
        /// NOTE: This will not save/update to the database.
        /// </remarks>
        /// <param name="accountId">Account ID to update the password for</param>
        /// <param name="newValidatedPassword">Password to hash and replace the old one with.</param>
        /// <param name="cancellationToken">Task cancellation token</param>
        /// <exception cref="Domain.Exceptions.ApiNotFoundException">Thrown when no account can be found using the specified account id.</exception>
        Task UpdatePasswordAsync(ulong accountId, string newValidatedPassword, CancellationToken cancellationToken = default);
        /// <summary>
        /// Update an account password.
        /// </summary>
        /// <remarks>
        /// NOTE: This will not save/update to the database.
        /// </remarks>
        /// <param name="account">Account to update the password for</param>
        /// <param name="newValidatedPassword">Password to hash and replace the old one with.</param>
        /// <param name="cancellationToken">Task cancellation token</param>
        Task UpdatePasswordAsync(Account account, string newValidatedPassword, CancellationToken cancellationToken = default);
        /// <summary>
        /// Validate a given password against the account password.
        /// </summary>
        /// <param name="accountId">Account ID</param>
        /// <param name="password">Password to hash and validate.</param>
        /// <param name="cancellationToken">Task cancellation token</param>
        /// <exception cref="Domain.Exceptions.ApiNotFoundException">Thrown when no account can be found using the specified account id.</exception>
        /// <returns>True if the password is valid.</returns>
        Task<bool> VerifyPasswordAsync(ulong accountId, string password, CancellationToken cancellationToken = default);
        /// <summary>
        /// Validate a given password against the account password.
        /// </summary>
        /// <param name="account">Account EF instance</param>
        /// <param name="password">Password to hash and validate.</param>
        /// <param name="cancellationToken">Task cancellation token</param>
        /// <returns>True if the password is valid.</returns>
        Task<bool> VerifyPasswordAsync(Account account, string password, CancellationToken cancellationToken = default);
    }
}
