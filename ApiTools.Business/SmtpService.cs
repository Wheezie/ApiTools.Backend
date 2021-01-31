using ApiTools.Business.Contracts;
using ApiTools.Domain;
using ApiTools.Domain.Exceptions;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace ApiTools.Business
{
    internal class SmtpService : ISmtpService
    {
        private readonly ISmtpClient smtpClient;
        private readonly ILogger<SmtpService> logger;
        private readonly IOptions<AppSettings> settings;
        private readonly IParserService parserService;

        private bool isDisposing = false;

        public SmtpService(ILogger<SmtpService> logger, IOptions<AppSettings> settings, ISmtpClient smtpClient = null)
        {
            this.smtpClient = smtpClient ?? new SmtpClient();
            this.settings = settings;
            this.logger = logger;
        }

        public async Task SendAsync(MimeMessage message, CancellationToken cancellationToken = default)
        {
            if (message.To.Count < 1)
            {
                logger.LogWarning("No email was send as no receivers were provided.");
            }
            else
            {
                if (message.From.Count < 1)
                {
                    message.From.Add(new MailboxAddress(settings.Value.Mail.Credentials.From, settings.Value.Mail.Credentials.From));
                }

                await EnsureConnection(cancellationToken).ConfigureAwait(false);

                try
                {
                    await smtpClient.SendAsync(message, cancellationToken)
                        .ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    LogErrorAndThrowException(ex, "Couldn't send mail to {Addressess}", message.To);
                }
            }
        }

        public Task SendAsync(string subject, MimePart body, string sender, string receiver, CancellationToken cancellationToken = default)
        {
            var mimeMessage = new MimeMessage
            {
                Subject = subject,
                Body = body
            };

            mimeMessage.From.Add(new MailboxAddress(sender, sender));
            mimeMessage.To.Add(new MailboxAddress(receiver, receiver));
            return SendAsync(mimeMessage, cancellationToken);
        }

        public Task SendAsync(string subject, string textBody, string sender, string receiver, CancellationToken cancellationToken = default)
            => SendAsync(subject, new TextPart("plain", textBody), sender, receiver, cancellationToken);

        public async Task EnsureConnection(CancellationToken cancellationToken = default)
        {
            var mailSettings = settings.Value.Mail;
            if (!cancellationToken.IsCancellationRequested && !smtpClient.IsConnected)
            {
                try
                {
                    await Connect(cancellationToken)
                        .ConfigureAwait(false);
                }
                catch (Exception ex) when (ex is ProtocolException || ex is SocketException)
                {
                    logger.LogError("Couldn't establish connection with the SMTP host {Host}, retrying in 15s.", mailSettings.Hostname);
#if DEBUG
                    if (XUnitRunner.IsTesting())
                    {
#endif
                    await Task.Delay(15)
                        .ConfigureAwait(false);
#if DEBUG
                    }
#endif
                    try
                    {
                        // Second attempt
                        await Connect(cancellationToken)
                            .ConfigureAwait(false);
                    }
                    catch (Exception ex2) when (ex is ProtocolException || ex is SocketException)
                    {
                        LogErrorAndThrowException(ex2, "Couldn't establish connection with the SMTP host {Host} after the second attempt.", mailSettings.Hostname);
                    }
                }

                logger.LogDebug("(Re-)established connection with the SMTP host {Host}", mailSettings.Hostname);

                try
                {
                    await smtpClient.AuthenticateAsync(
                            mailSettings.Credentials.Username,
                            mailSettings.Credentials.Password,
                            cancellationToken)
                        .ConfigureAwait(false);
                    logger.LogDebug("Authenticated on the SMTP host {Host}", mailSettings.Hostname);
                }
                catch (Exception ex) when (ex is AuthenticationException || ex is SaslException)
                {
                    LogErrorAndThrowException(ex, "Authentication failed with the SMTP host {Host}", mailSettings.Hostname);
                }
            }
        }
        public void Dispose()
        {
            if (!isDisposing)
            {
                isDisposing = true;
                smtpClient.Dispose();
            }
        }

        private Task Connect(CancellationToken cancellationToken = default)
            => smtpClient.ConnectAsync(
                    settings.Value.Mail.Hostname,
                    settings.Value.Mail.Port,
                    settings.Value.Mail.Ssl,
                    cancellationToken);

        private void LogErrorAndThrowException(Exception exception, string message, params object[] args)
        {
            logger.LogError(exception, message, args);
            throw new SmtpException(message, exception);
        }
    }
}
