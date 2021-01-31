using ApiTools.Business.Contracts;
using ApiTools.Domain;
using ApiTools.Domain.Exceptions;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ApiTools.Business.Tests
{
    public class SmtpServiceTests : IDisposable
    {
        private readonly Mock<ILogger<SmtpService>> loggerMock;
        private readonly Mock<IOptions<AppSettings>> optionsMock;
        private readonly Mock<ISmtpClient> smtpClientMock;

        private SmtpService service;
        
        // Setup
        public SmtpServiceTests()
        {
            loggerMock = new Mock<ILogger<SmtpService>>();

            optionsMock = new Mock<IOptions<AppSettings>>();
            optionsMock.Setup(x => x.Value)
                .Returns(new AppSettings
                {
                    Mail = new Domain.Options.SmtpSettings
                    {
                        Credentials = new MailCredential
                        {
                            From = "noreply@domain.tld",
                            Username = "apitools@domain.local",
                            Password = "SecurePassword"
                        },
                        Hostname = "mail.domain.local",
                        Port = 587,
                        Ssl = false,
                    }
                });

            smtpClientMock = new Mock<ISmtpClient>();
        }

        #region EnsureConnection
        [Fact]
        public async Task EnsureConnection_Valid()
        {
            // Arrange
            var hostname = "mail.domain.local";
            bool connectCalled = false;
            bool authCalled = false;
            smtpClientMock.Setup(x => x.ConnectAsync(hostname, 587, false, It.IsAny<CancellationToken>()))
                .Returns(() => Task.Run(() => connectCalled = true));
            smtpClientMock.Setup(x => x.AuthenticateAsync("apitools@domain.local", "SecurePassword", It.IsAny<CancellationToken>()))
                .Returns(() => Task.Run(() => authCalled = true));
            SetupSmtpService(false);

            // Act
            await service.EnsureConnection();

            // Assert
            loggerMock.CatchLog(hostname, LogLevel.Debug, Times.Exactly(2));
            Assert.True(connectCalled, "The ConnectAsync function wasn't called (or didn't use the correct arguments)");
            Assert.True(authCalled, "The AuthenticateAsync function wasn't called (or didn't use the correct arguments)");
        }

        [Fact]
        public async Task EnsureConnection_Connection_FirstFailed()
        {
            // Arrange
            var hostname = "mail.domain.local";
            bool connectCalled = false;
            bool authCalled = false;
            smtpClientMock.SetupSequence(x => x.ConnectAsync(hostname, 587, false, It.IsAny<CancellationToken>()))
                .Throws(new System.Net.Sockets.SocketException())
                .Returns(() => Task.Run(() => connectCalled = true));
            smtpClientMock.Setup(x => x.AuthenticateAsync("apitools@domain.local", "SecurePassword", It.IsAny<CancellationToken>()))
                .Returns(() => Task.Run(() => authCalled = true));
            SetupSmtpService(false);

            // Act
            await service.EnsureConnection();

            // Assert
            loggerMock.CatchLog($"Couldn't establish connection with the SMTP host {hostname}, retrying in 15s.", LogLevel.Error, Times.Once());
            Assert.True(connectCalled, "The ConnectAsync function wasn't called twice (or didn't use the correct arguments)");
            Assert.True(authCalled, "The AuthenticateAsync function wasn't called (or didn't use the correct arguments)");
        }

        [Fact]
        public async Task EnsureConnection_Connection_SecondFailedAlso()
        {
            // Arrange
            var hostname = "mail.domain.local";
            smtpClientMock.SetupSequence(x => x.ConnectAsync(hostname, 587, false, It.IsAny<CancellationToken>()))
                .Throws(new System.Net.Sockets.SocketException())
                .Throws(new SmtpProtocolException());
            SetupSmtpService(false);

            // Act & Assert
            await Assert.ThrowsAsync<SmtpException>(() => service.EnsureConnection());
            loggerMock.CatchExceptionLog<SmtpService, SmtpProtocolException>($"Couldn't establish connection with the SMTP host {hostname} after the second attempt.", LogLevel.Error, Times.Once());
        }

        [Fact]
        public async Task EnsureConnection_AuthenticationFailed()
        {
            // Arrange
            var hostname = "mail.domain.local";
            smtpClientMock.SetupSequence(x => x.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Throws(new AuthenticationException())
                .Throws(new SaslException("", SaslErrorCode.ChallengeTooLong, ""));
            SetupSmtpService(false);

            // Act & Assert 1
            await Assert.ThrowsAsync<SmtpException>(() => service.EnsureConnection());
            loggerMock.CatchExceptionLog<SmtpService, AuthenticationException>($"Authentication failed with the SMTP host {hostname}", LogLevel.Error, Times.Once());

            // Act & Assert 2
            await Assert.ThrowsAsync<SmtpException>(() => service.EnsureConnection());
            loggerMock.CatchExceptionLog<SmtpService, SaslException>($"Authentication failed with the SMTP host {hostname}", LogLevel.Error, Times.Once());
        }

        #endregion
        #region SendAsync
        [Fact]
        public async Task SendAsync_MimeMessage_Valid()
        {
            // Arrange
            var settings = optionsMock.Object.Value.Mail;
            MimeMessage message = new MimeMessage
            {
                Subject = "Mail subject",
                Body = new TextPart("Mail body contents")
            };

            message.From.Add(new MailboxAddress(settings.Credentials.From, settings.Credentials.From));
            message.To.Add(new MailboxAddress("receiver@domain.tld", "receiver@domain.tld"));

            bool sent = false;
            smtpClientMock.Setup(x => x.SendAsync(message, It.IsAny<CancellationToken>(), It.IsAny<ITransferProgress>()))
                .Returns(() => Task.Run(() => sent = true));
            SetupSmtpService();

            // Act
            await service.SendAsync(message);

            // Assert
            Assert.True(sent, "The internal SendAsync function wasn't called (or didn't use the correct arguments)");
        }

        [Fact]
        public async Task SendAsync_MimeMessage_NoReceiver()
        {
            // Arrange
            var settings = optionsMock.Object.Value.Mail;
            MimeMessage message = new MimeMessage
            {
                Subject = "Mail subject",
                Body = new TextPart("Mail body contents")
            };

            message.From.Add(new MailboxAddress(settings.Credentials.From, settings.Credentials.From));

            bool sent = false;
            smtpClientMock.Setup(x => x.SendAsync(It.IsAny<MimeMessage>(), It.IsAny<CancellationToken>(), It.IsAny<ITransferProgress>()))
                .Returns(() => Task.Run(() => sent = true));
            SetupSmtpService();

            // Act
            await service.SendAsync(message);

            // Assert
            loggerMock.CatchLog("No email was send as no receivers were provided.", LogLevel.Warning, Times.Once());
            Assert.False(sent, "The internal SendAsync function shouldn't be called.");
        }

        [Fact]
        public async Task SendAsync_MimeMessage_NoSender()
        {
            // Arrange
            var settings = optionsMock.Object.Value.Mail;
            MimeMessage message = new MimeMessage
            {
                Subject = "Mail subject",
                Body = new TextPart("Mail body contents")
            };

            message.To.Add(new MailboxAddress("receiver@domain.tld", "receiver@domain.tld"));

            bool sent = false;
            smtpClientMock.Setup(x => x.SendAsync(It.IsAny<MimeMessage>(), It.IsAny<CancellationToken>(), It.IsAny<ITransferProgress>()))
                .Returns(() => Task.Run(() => sent = true));
            SetupSmtpService();

            // Act
            await service.SendAsync(message);

            // Assert
            Assert.True(sent, "The internal SendAsync function should be called.");
            Assert.Equal(settings.Credentials.From, ((MailboxAddress)message.From[0]).Address);
        }

        [Fact]
        public async Task SendAsync_TextPart_Valid()
        {
            // Arrange
            var settings = optionsMock.Object.Value.Mail;
            var subject = "Mail subject";
            var body = new TextPart("An email body");
            var receiver = "receiver@domain.tld";

            bool sent = false;
            MimeMessage returnMessage = null;
            smtpClientMock.Setup(x => x.SendAsync(It.IsAny<MimeMessage>(), It.IsAny<CancellationToken>(), It.IsAny<ITransferProgress>()))
                .Callback<MimeMessage, CancellationToken, ITransferProgress>((message, _, __) =>
                {
                    returnMessage = message;
                })
                .Returns(() => Task.Run(() => sent = true));
            SetupSmtpService();

            // Act
            await service.SendAsync(subject, body, settings.Credentials.From, receiver);

            // Assert
            Assert.True(sent, "The SendAsync function wasn't called (or didn't use the correct arguments)");
            Assert.Equal(subject, returnMessage?.Subject);
            Assert.Equal(body, returnMessage?.Body);
            Assert.Equal(receiver, ((MailboxAddress)returnMessage?.To[0])?.Address);
            Assert.Equal(settings.Credentials.From, ((MailboxAddress)returnMessage?.From[0])?.Address);
        }

        [Fact]
        public async Task SendAsync_Strings_Valid()
        {
            // Arrange
            var settings = optionsMock.Object.Value.Mail;
            var subject = "Mail subject";
            var body = "An email body";
            var receiver = "receiver@domain.tld";

            bool sent = false;
            MimeMessage returnMessage = null;
            smtpClientMock.Setup(x => x.SendAsync(It.IsAny<MimeMessage>(), It.IsAny<CancellationToken>(), It.IsAny<ITransferProgress>()))
                .Callback<MimeMessage, CancellationToken, ITransferProgress>((message, _, __) =>
                {
                    returnMessage = message;
                })
                .Returns(() => Task.Run(() => sent = true));
            SetupSmtpService();

            // Act
            await service.SendAsync(subject, body, settings.Credentials.From, receiver);

            // Assert
            Assert.True(sent, "The SendAsync function wasn't called (or didn't use the correct arguments)");

            Assert.Equal(subject, returnMessage?.Subject);
            Assert.Equal(body, ((TextPart)returnMessage?.Body).Text);
            Assert.Equal(receiver, ((MailboxAddress)returnMessage?.To[0])?.Address);
            Assert.Equal(settings.Credentials.From, ((MailboxAddress)returnMessage?.From[0])?.Address);
        }

        [Fact]
        public async Task SendAsync_SendException()
        {
            // Arrange
            var settings = optionsMock.Object.Value.Mail;
            MimeMessage message = new MimeMessage
            {
                Subject = "Mail subject",
                Body = new TextPart("Mail body contents")
            };

            message.To.Add(new MailboxAddress("receiver@domain.tld", "receiver@domain.tld"));

            smtpClientMock.Setup(x => x.SendAsync(It.IsAny<MimeMessage>(), It.IsAny<CancellationToken>(), It.IsAny<ITransferProgress>()))
                .Throws(new Exception());
            SetupSmtpService();

            // Act & Assert
            await Assert.ThrowsAsync<SmtpException>(() => service.SendAsync(message));
            loggerMock.CatchExceptionLog<SmtpService, Exception>("Couldn't send mail to \"receiver@domain.tld\"", LogLevel.Error, Times.Once());
        }
        #endregion

        private void SetupSmtpService(bool setupSmtpConnection = true)
        {
            if (setupSmtpConnection)
            {
                smtpClientMock.Setup(x => x.ConnectAsync("mail.domain.local", 587, false, It.IsAny<CancellationToken>()))
                    .Returns(() => Task.Run(() => { }));
                smtpClientMock.Setup(x => x.AuthenticateAsync("apitools@domain.local", "SecurePassword", It.IsAny<CancellationToken>()))
                    .Returns(() => Task.Run(() => { }));
            }

            service = new SmtpService(loggerMock.Object, optionsMock.Object, smtpClientMock.Object);
        }

        public void Dispose()
        {
            service.Dispose();
        }
    }
}
