using ApiTools.Domain.Data;
using System;
using System.Threading;
using System.Threading.Tasks;
using Wangkanai.Detection.Models;

namespace ApiTools.Business.Contracts
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// Attempt to authenticate an user using a provided username and password.
        /// </summary>
        /// <param name="usernameOrEmail">Account username (or email) identifier</param>
        /// <param name="password">Account password</param>
        /// <param name="browser">Browser the session is authenticating with</param>
        /// <param name="platform">Platform the session is authenticating from</param>
        /// <param name="ipAddress">IP address to link with the session</param>
        /// <param name="cancellationToken">Request cancellation token</param>
        /// <returns>A newly created AccountSession if succeeded</returns>
        Task<AccountSession> AuthenticateAsync(string usernameOrEmail, string password, Browser browser, Platform platform, byte[] ipAddress, CancellationToken cancellationToken = default);
        /// <summary>
        /// Generate a link to allow for resetting a password.
        /// </summary>
        /// <param name="accountId">Account identifier</param>
        /// <param name="cancellationToken">Request cancellation token</param>
        /// <returns>The reset uri</returns>
        Task<string> GetResetPasswordLinkAsync(ulong accountId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Generate a link to allow for resetting a password.
        /// </summary>
        /// <param name="account">Account to generate the url from</param>
        /// <param name="cancellationToken">Request cancellation token</param>
        /// <returns>The reset uri</returns>
        Task<string> GetResetPasswordLinkAsync(Account account, CancellationToken cancellationToken = default);
        /// <summary>
        /// Generate a session token
        /// </summary>
        /// <param name="sessionId">Account session id to fetch the session contents from</param>
        /// <param name="cancellationToken">Request cancellation token</param>
        /// <returns>An authentication token</returns>
        Task<string> GetSessionToken(Guid sessionId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Generate a session token
        /// </summary>
        /// <param name="session">Account session to generate from</param>
        /// <param name="cancellationToken">Request cancellation token</param>
        /// <returns>An authentication token</returns>
        Task<string> GetSessionToken(AccountSession session, CancellationToken cancellationToken = default);
        /// <summary>
        /// Sign out all sessions for a user
        /// </summary>
        /// <param name="accountId">Account identifier</param>
        /// <param name="cancellationToken">Request cancellation token</param>
        Task LogoutAllSessions(ulong accountId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Sign out all sessions for a user
        /// </summary>
        /// <param name="account">Account instance</param>
        /// <param name="cancellationToken">Request cancellation token</param>
        Task LogoutAllSessions(Account account, CancellationToken cancellationToken = default);
        /// <summary>
        /// Sign out a specific session for a user
        /// </summary>
        /// <param name="sessionId">Session identifier (most likely the Jti)</param>
        /// <param name="cancellationToken">Request cancellation token</param>
        Task LogoutSession(Guid sessionId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Sign out a specific session for a user
        /// </summary>
        /// <param name="session">Account session instance</param>
        /// <param name="cancellationToken">Request cancellation token</param>
        Task LogoutSession(AccountSession session, CancellationToken cancellationToken = default);
        /// <summary>
        /// Send a password reset email to the primary email of the account based on an account ID.
        /// </summary>
        /// <param name="accountId">Account identifier</param>
        /// <param name="cancellationToken">Request cancellation token</param>
        /// <returns>A newly created AccountSession if succeeded</returns>
        Task ResetPasswordAsync(ulong accountId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Send a password reset email to the primary email of the account based on an account ID.
        /// </summary>
        /// <param name="account">Account object to use </param>
        /// <param name="cancellationToken">Request cancellation token</param>
        /// <returns>A newly created AccountSession if succeeded</returns>
        Task ResetPasswordAsync(Account account, CancellationToken cancellationToken = default);
    }
}
