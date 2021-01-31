using MimeKit;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ApiTools.Domain.Exceptions;

namespace ApiTools.Business.Contracts
{
    public interface ISmtpService : IDisposable
    {
        /// <summary>
        /// Send a mail message over smtp
        /// </summary>
        /// <param name="message">Mime message format</param>
        /// <param name="cancellationToken">Task cancellation token</param>
        /// <exception cref="SmtpException">When an error occurred sending the email</exception>
        /// <exception cref="OperationCanceledException">Operation token was cancelled</exception>
        Task SendAsync(MimeMessage message, CancellationToken cancellationToken = default);
        /// <summary>
        /// Send a mail message over smtp
        /// </summary>
        /// <param name="subject">Mail subject</param>
        /// <param name="body">Mail contents</param>
        /// <param name="sender">Sender mail address</param>
        /// <param name="receiver">Receiver mail address</param>
        /// <param name="cancellationToken">Task cancellation token</param>
        /// <exception cref="SmtpException">When an error occurred sending the email</exception>
        /// <exception cref="OperationCanceledException">Operation token was cancelled</exception>
        Task SendAsync(string subject, MimePart body, string sender, string receiver, CancellationToken cancellationToken = default);
        /// <summary>
        /// Send a mail message over smtp
        /// </summary>
        /// <param name="subject">Mail subject</param>
        /// <param name="textBody">Mail contents</param>
        /// <param name="sender">Sender mail address</param>
        /// <param name="receiver">Receiver mail address</param>
        /// <param name="cancellationToken">Task cancellation token</param>
        /// <exception cref="SmtpException">An error occurred sending the email</exception>
        /// <exception cref="OperationCanceledException">Operation token was cancelled</exception>
        Task SendAsync(string subject, string textBody, string sender, string receiver, CancellationToken cancellationToken = default);
        /// <summary>
        /// Retrieve an html body from a file and format it using.
        /// </summary>
        /// <param name="filePath">Html template storage path</param>
        /// <param name="replace">(Interpolated) keys inside the HTML template to be replace with the values.</param>
        /// <param name="cancellationToken">Task cancellation token</param>
        /// <exception cref="ArgumentException">When invalid arguments are provided</exception>
        /// <exception cref="System.IO.IOException">An error occurred parsing/reading the template</exception>
        /// <exception cref="OperationCanceledException">Operation token was cancelled</exception>
        /// <returns>MimeMessage with the parsed mail body (formated)</returns>
        Task<MimeMessage> FetchHtmlBody(string filePath, IDictionary<string, string> replace, CancellationToken cancellationToken = default);
    }
}
