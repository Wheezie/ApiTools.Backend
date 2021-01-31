using MimeKit;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ApiTools.Business.Contracts
{
    public interface ISmtpService : IDisposable
    {
        /// <summary>
        /// Send a mail message over smtp
        /// </summary>
        /// <param name="message">Mime message format</param>
        Task SendAsync(MimeMessage message, CancellationToken cancellationToken = default);
        /// <summary>
        /// Send a mail message over smtp
        /// </summary>
        /// <param name="subject">Mail subject</param>
        /// <param name="body">Mail contents</param>
        /// <param name="sender">Sender mail address</param>
        /// <param name="receiver">Receiver mail address</param>
        Task SendAsync(string subject, TextPart body, string sender, string receiver, CancellationToken cancellationToken = default);
        /// <summary>
        /// Send a mail message over smtp
        /// </summary>
        /// <param name="subject">Mail subject</param>
        /// <param name="body">Mail contents</param>
        /// <param name="sender">Sender mail address</param>
        /// <param name="receiver">Receiver mail address</param>
        Task SendAsync(string subject, string body, string sender, string receiver, CancellationToken cancellationToken = default);
    }
}
