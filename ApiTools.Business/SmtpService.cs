using ApiTools.Business.Contracts;
using ApiTools.Domain;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
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

                await EnsureConnection().ConfigureAwait(false);

                try
                {
                    await smtpClient.SendAsync(message, cancellationToken)
                        .ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Couldn't send mail to {Addressess}", message.To);
                    throw;
                }
            }
        }

        public Task SendAsync(string subject, TextPart body, string sender, string receiver, CancellationToken cancellationToken = default)
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

        public Task SendAsync(string subject, string body, string sender, string receiver, CancellationToken cancellationToken = default)
            => SendAsync(subject, new TextPart("plain", body), sender, receiver, cancellationToken);

        public async Task EnsureConnection()
        {
            var mailSettings = settings.Value.Mail;
            if (!smtpClient.IsConnected)
            {
                try
                {
                    await Connect()
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
                        await Connect()
                        .ConfigureAwait(false);
                    }
                    catch (Exception ex2) when (ex is ProtocolException || ex is SocketException)
                    {
                        logger.LogError(ex2, "Couldn't establish connection with the SMTP host {Host} after the second attempt.", mailSettings.Hostname);
                        throw;
                    }
                }

                logger.LogDebug("(Re-)established connection with the SMTP host {Host}", mailSettings.Hostname);

                try
                {
                    await smtpClient.AuthenticateAsync(
                            mailSettings.Credentials.Username,
                            mailSettings.Credentials.Password)
                        .ConfigureAwait(false);
                    logger.LogDebug("Authenticated on the SMTP host {Host}", mailSettings.Hostname);
                }
                catch (Exception ex) when (ex is AuthenticationException || ex is SaslException)
                {
                    logger.LogError(ex, "Authentication failed with the SMTP host {Host}", mailSettings.Hostname);
                    throw;
                }
            }
        }

        private Task Connect()
            => smtpClient.ConnectAsync(
                    settings.Value.Mail.Hostname,
                    settings.Value.Mail.Port,
                    settings.Value.Mail.Ssl);

        public void Dispose()
        {
            if (!isDisposing)
            {
                isDisposing = true;

                smtpClient.Dispose();
            }
        }
    }
}
