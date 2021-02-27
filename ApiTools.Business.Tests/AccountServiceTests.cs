using ApiTools.Data;
using ApiTools.Domain;
using ApiTools.Domain.Data;
using ApiTools.Domain.Exceptions;
using ApiTools.Domain.Options;
using ApiTools.Domain.Options.Fields;
using ApiTools.Domain.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ApiTools.Business.Tests
{
    public class AccountServiceTests : IDisposable
    {
        private readonly Mock<ILogger<AccountService>> loggerMock;
        private readonly Mock<IOptions<AppSettings>> optionsMock;
        private readonly Mock<IPasswordHasher<Account>> passwordHasherMock;
        private readonly ApiDbContext dbContext;

        private AccountService service;

        public AccountServiceTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder<ApiDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            dbContext = new ApiDbContext(dbContextOptions);

            loggerMock = new Mock<ILogger<AccountService>>();

            optionsMock = new Mock<IOptions<AppSettings>>();
            optionsMock.Setup(x => x.Value)
                .Returns(new AppSettings
                {
                    Mail = new SmtpSettings
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
                    },
                    Limits = new LimitsSettings
                    {
                        Account = new AccountLimits
                        {
                            Email = new EmailField(),
                            FirstName = new RequiredField
                            {
                                Maximum = 32,
                                Minimum = 2
                            },
                            LastName = new RequiredField
                            {
                                Maximum = 32,
                                Minimum = 2
                            },
                            Username = new RequiredField
                            {
                                Maximum = 32,
                                Minimum = 2
                            },
                            Password = new PasswordField
                            {
                                Minimum = 6,
                                Number = true,
                                SpecialCharacter = true
                            }
                        }
                    },
                    Security = new SecuritySettings
                    {
                        Invite = new InviteSettings
                        {
                            Lifetime = TimeSpan.FromDays(14)
                        }
                    }
                });

            passwordHasherMock = new Mock<IPasswordHasher<Account>>();
            service = new AccountService(optionsMock.Object, loggerMock.Object, dbContext, passwordHasherMock.Object);
        }

        #region Constructor
        [Fact]
        public void Constructor_NoSettings()
        {
            // Act & Assert
            var result = Assert.Throws<ArgumentException>(() => new AccountService(null, null, null, null));
            Assert.Equal("No AppSettings options found!", result.Message);
        }
        #endregion
        #region CreateEmailAsync
        [Fact]
        public async Task CreateEmailAsync_AlreadyInUse()
        {
            // Arrange
            var account = SetupTestAccount();

            // Act
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => service.CreateEmailAsync("user@domain.tld"));

            // Assert
            Assert.Equal("The provided email is already used.", exception.Message);
        }
        [Fact]
        public async Task CreateEmailAsync_EmailBanned()
        {
            // Arrange
            var email = new AccountEmail
            {
                Email = "user@domain.tld",
                Type = EmailType.Primary | EmailType.Banned,
                Verified = true
            };
            dbContext.Add(email);
            dbContext.SaveChanges();

            // Act
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => service.CreateEmailAsync("user@domain.tld"));

            // Assert
            Assert.Equal("The provided email is already used.", exception.Message);
        }
        [Fact]
        public async Task CreateEmailAsync_ReuseUnassigned()
        {
            // Arrange
            var email = new AccountEmail
            {
                Email = "user@domain.tld",
                Type = EmailType.Additional,
                Verified = true
            };
            dbContext.Add(email);
            dbContext.SaveChanges();

            // Act
            var result = await service.CreateEmailAsync("user@domain.tld");

            // Assert
            Assert.Equal("user@domain.tld", result.Email);
            Assert.Equal(EmailType.Primary, result.Type);
            Assert.False(result.Verified);
        }
        [Fact]
        public async Task CreateEmailAsync_NewlyCreated()
        {
            // Arrange

            // Act
            var result = await service.CreateEmailAsync("user@domain.tld");

            // Assert
            Assert.Equal("user@domain.tld", result.Email);
            Assert.Equal(EmailType.Primary, result.Type);
            Assert.False(result.Verified);
        }
        #endregion
        #region CreateNewInviteAsync
        [Fact]
        public async Task CreateNewInviteAsync_Valid_WithExpire()
        {
            // Arrange
            SetupTestAccount();
            var dateExpire = DateTime.UtcNow.AddDays(1);
            var dateCreated = DateTime.UtcNow;

            // Act
            var result = await service.CreateNewInviteAsync(10, dateExpire);

            // Assert
            Assert.Equal((ulong)10, result.InviterId);
            Assert.Null(result.AcceptorId);
            Assert.Equal(dateCreated, result.Invited, TimeSpan.FromSeconds(1));
            Assert.Equal(dateExpire, result.Expire);
        }
        [Fact]
        public async Task CreateNewInviteAsync_Valid_WithoutExpire()
        {
            // Arrange
            SetupTestAccount();
            var dateExpire = DateTime.UtcNow.AddDays(14);

            // Act
            var result = await service.CreateNewInviteAsync(10);

            // Assert
            Assert.Equal(dateExpire, result.Expire, TimeSpan.FromSeconds(1));
        }
        #endregion
        #region GetAccountAsync
        [Fact]
        public async Task GetAccountAsync_Valid_ById()
        {
            // Arrange
            SetupTestAccount();
            var account = dbContext.Users.First(x => x.Id == 10);

            // Act
            var result = await service.GetAccountAsync(10);

            // Assert
            Assert.Equal(account, result);
        }
        [Theory]
        [InlineData("username")]
        [InlineData("user@domain.tld")]
        public async Task GetAccountAsync_Valid_ByUsernameOrEmail(string usernameOrEmail)
        {
            // Arrange
            SetupTestAccount();
            var account = dbContext.Users.First(x => x.Id == 10);

            // Act
            var result = await service.GetAccountAsync(usernameOrEmail);

            // Assert
            Assert.Equal(account, result);
        }
        [Theory]
        [InlineData("username")]
        [InlineData("user@domain.tld")]
        public async Task GetAccountAsync_NonExistant_ByUsernameOrEmail(string usernameOrEmail)
        {
            // Act
            var result = await service.GetAccountAsync(usernameOrEmail);

            // Assert
            Assert.Null(result);
        }
        [Fact]
        public async Task GetAccountAsync_NonExistant_ById()
        {
            // Act
            var result = await service.GetAccountAsync(10);

            // Assert
            Assert.Null(result);
        }
        #endregion
        #region GetInviteTokenAsync
        [Fact]
        public async Task GetInviteTokenAsync_Valid_WithoutAccountIds()
        {
            // Arrange
            var invite = new AccountInvite
            {
                Invited = DateTime.UtcNow,
                InviterId = 10,
                Expire = DateTime.UtcNow.AddDays(14)
            };
            dbContext.Invites.Add(invite);
            dbContext.SaveChanges();

            // Act
            var result = await service.GetInviteTokenAsync(invite.Token);

            // Assert
            Assert.Equal(invite, result);
        }
        [Fact]
        public async Task GetInviteTokenAsync_NotFound_WithoutAccountIds()
        {
            // Act
            var result = await service.GetInviteTokenAsync(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }
        [Theory]
        [InlineData(10)]
        [InlineData(11)]
        public async Task GetInviteTokenAsync_Valid_WithAccountIds(ulong accountId)
        {
            // Arrange
            var invite = new AccountInvite
            {
                Invited = DateTime.UtcNow,
                InviterId = 10,
                Expire = DateTime.UtcNow.AddDays(14),
                AcceptorId = 11
            };
            dbContext.Invites.Add(invite);
            dbContext.SaveChanges();

            // Act
            var result = await service.GetInviteTokenAsync(invite.Token, accountId);

            // Assert
            Assert.Equal(invite, result);
        }
        [Theory]
        [InlineData(1)]
        [InlineData(12)]
        public async Task GetInviteTokenAsync_NotFound_WithAccountIds(ulong accountId)
        {
            // Arrange
            var invite = new AccountInvite
            {
                Invited = DateTime.UtcNow,
                InviterId = 10,
                Expire = DateTime.UtcNow.AddDays(14),
                AcceptorId = 11
            };
            dbContext.Invites.Add(invite);
            dbContext.SaveChanges();

            // Act
            var result = await service.GetInviteTokenAsync(invite.Token, accountId);

            // Assert
            Assert.Null(result);
        }
        [Theory]
        [InlineData(10)]
        [InlineData(11)]
        public async Task GetInviteTokenAsync_NotFound_InvlaidToken(ulong accountId)
        {
            // Arrange
            var invite = new AccountInvite
            {
                Invited = DateTime.UtcNow,
                InviterId = 10,
                Expire = DateTime.UtcNow.AddDays(14),
                AcceptorId = 11
            };
            dbContext.Invites.Add(invite);
            dbContext.SaveChanges();

            // Act
            var result = await service.GetInviteTokenAsync(Guid.NewGuid(), accountId);

            // Assert
            Assert.Null(result);
        }
        #endregion
        #region GetInviteTokensAsync
        [Fact]
        public async Task GetInviteTokensAsync_Valid()
        {
            // Arrange
            int inviteCount = new Random().Next(0, 30);
            List<AccountInvite> invites = new List<AccountInvite>();
            for (int i = 0; i < inviteCount; i++)
            {
                var invite = new AccountInvite
                {
                    Invited = DateTime.UtcNow,
                    InviterId = 10,
                    Expire = DateTime.UtcNow.AddDays(14),
                    AcceptorId = 11
                };
                invites.Add(invite);
                dbContext.Invites.Add(invite);
            }
            dbContext.SaveChanges();

            // Act
            var result = await service.GetInviteTokensAsync(10);

            // Assert
            Assert.Equal(invites, result);
        }

        [Fact]
        public async Task GetInviteTokensAsync_Valid_Empty()
        {
            // Act
            var result = await service.GetInviteTokensAsync(10);

            // Assert
            Assert.Empty(result);
        }
        #endregion
        #region RegisterAccountAsync
        [Fact]
        public async Task RegisterAccountAsync_Valid()
        {
            // Arrange
            var regRequest = new RegistrationRequest
            {
                Username = "validUsername",
                FirstName = "validFirstName",
                LastName = "validLastName",
                Email = "valid@domain.tld",
                Password = "validP@ssw0rd"
            };

            // Act
            var badFields = await service.RegisterAccountAsync(regRequest);

            // Assert
            Assert.Empty(badFields);
        }

        [Fact]
        public async Task RegisterAccountAsync_FieldsToShort()
        {
            // Arrange
            var limits = optionsMock.Object.Value.Limits.Account;
            var regRequest = new RegistrationRequest
            {
                Username = string.Join("", Enumerable.Repeat(" ", limits.Username.Maximum - 1)),
                FirstName = string.Join("", Enumerable.Repeat(" ", limits.Username.Maximum - 1)),
                LastName = "validLastName",
                Email = "valid@domain.tld",
                Password = "validP@ssw0rd"
            };

            // Act
            var badFields = await service.RegisterAccountAsync(regRequest);

            // Assert
            Assert.NotEmpty(badFields);
        }
        #endregion
        #region UpdatePasswordAsync
        [Fact]
        public async Task UpdatePasswordAsync_WithAccountId()
        {
            // Arrange
            var account = SetupTestAccount();
            passwordHasherMock.Setup(x => x.HashPassword(account, It.IsAny<string>()))
                .Returns("newPasswordHash");

            // Act
            await service.UpdatePasswordAsync(account.Id, "newPassword");

            // Assert
            Assert.Equal("newPasswordHash", account.PasswordHash);
        }

        [Fact]
        public async Task UpdatePasswordAsync_WithAccountId_NotFound()
        {
            // Act
            var result = await Assert.ThrowsAsync<ApiNotFoundException>(() => service.UpdatePasswordAsync(1, "somePassword"));

            // Assert
            Assert.False(result.Logged);
            Assert.Equal("Couldn't find the account with the specified Account Id", result.Message);
        }

        [Fact]
        public async Task UpdatePasswordAsync_Valid()
        {
            // Arrange
            Account account = new Account
            {
                PasswordHash = "somePasswordHash"
            };

            passwordHasherMock.Setup(x => x.HashPassword(account, "newPassword"))
                .Returns("newPasswordHash");

            // Act
            await service.UpdatePasswordAsync(account, "newPassword");

            // Assert
            Assert.Equal("newPasswordHash", account.PasswordHash);
        }
        #endregion
        #region VerifyPasswordAsync
        [Fact]
        public async Task VerifyPasswordAsync_WithAccountId()
        {
            // Arrange
            var account = SetupTestAccount();

            passwordHasherMock.Setup(x => x.VerifyHashedPassword(It.IsAny<Account>(), account.PasswordHash, "somePassword"))
                .Returns(PasswordVerificationResult.SuccessRehashNeeded);
            passwordHasherMock.Setup(x => x.HashPassword(It.IsAny<Account>(), "somePassword"))
                .Returns("newPasswordHash");

            // Act
            var result = await service.VerifyPasswordAsync(account.Id, "somePassword");

            // Assert
            Assert.True(result);
            Assert.Equal("newPasswordHash", account.PasswordHash);
        }
        [Fact]
        public async Task VerifyPasswordAsync_WithAccountId_NotFound()
        {
            // Act
            var result = await Assert.ThrowsAsync<ApiNotFoundException>(() => service.VerifyPasswordAsync(1, "somePassword"));

            // Assert
            Assert.False(result.Logged);
            Assert.Equal("Couldn't find the account with the specified Account Id", result.Message);
        }

        [Fact]
        public async Task VerifyPasswordAsync_Success()
        {
            // Assert
            Account account = new Account
            {
                PasswordHash = "oldPasswordHash"
            };
            passwordHasherMock.Setup(x => x.VerifyHashedPassword(account, "oldPasswordHash", "somePassword"))
                .Returns(PasswordVerificationResult.Success);

            // Act
            var result = await service.VerifyPasswordAsync(account, "somePassword");

            // Assert
            Assert.True(result);
            Assert.Equal("oldPasswordHash", account.PasswordHash);
        }

        [Fact]
        public async Task VerifyPasswordAsync_Failed()
        {
            // Assert
            Account account = new Account
            {
                PasswordHash = "oldPasswordHash"
            };
            passwordHasherMock.Setup(x => x.VerifyHashedPassword(account, "oldPasswordHash", "somePassword"))
                .Returns(PasswordVerificationResult.Failed);

            // Act
            var result = await service.VerifyPasswordAsync(account, "somePassword");

            // Assert
            Assert.False(result);
            Assert.Equal("oldPasswordHash", account.PasswordHash);
        }
        [Fact]
        public async Task VerifyPasswordAsync_SuccessNeedsRehash()
        {
            // Assert
            Account account = new Account
            {
                PasswordHash = "oldPasswordHash"
            };
            passwordHasherMock.Setup(x => x.VerifyHashedPassword(account, "oldPasswordHash", "somePassword"))
                .Returns(PasswordVerificationResult.SuccessRehashNeeded);
            passwordHasherMock.Setup(x => x.HashPassword(account, "somePassword"))
                .Returns("newPasswordHash");

            // Act
            var result = await service.VerifyPasswordAsync(account, "somePassword");

            // Assert
            Assert.True(result);
            Assert.Equal("newPasswordHash", account.PasswordHash);
        }
        #endregion
        public void Dispose()
        {
            dbContext.Dispose();
        }

        private Account SetupTestAccount()
        {
            Account account = new Account
            {
                Id = 10,
                UserName = "username",
                NormalizedUserName = "USERNAME",
                FirstName = "firstName",
                LastName = "lastName",
                Email = "user@domain.tld",
                PrimaryEmail = new AccountEmail
                {
                    AccountId = 10,
                    Email = "user@domain.tld",
                    Type = EmailType.Primary,
                    Verified = true
                },
                PasswordHash = "somePasswordHash"
            };
            dbContext.Add(account);
            dbContext.SaveChanges();

            return account;
        }
    }
}
