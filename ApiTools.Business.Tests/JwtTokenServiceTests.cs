using ApiTools.Domain;
using ApiTools.Domain.Options;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;
using System.Linq;

namespace ApiTools.Business.Tests
{
    public class JwtTokenServiceTests
    {
        private readonly Mock<IOptions<AppSettings>> optionsMock;
        private readonly JwtTokenService tokenService;

        public JwtTokenServiceTests()
        {
            optionsMock = new Mock<IOptions<AppSettings>>();
            optionsMock.Setup(x => x.Value)
                .Returns(new AppSettings
                {
                    Security = new SecuritySettings
                    {
                        Token = new JWTTokenSettings
                        {
                            Audience = "testAudience",
                            Issuer = "testIssuer",
                            LifeTime = TimeSpan.FromHours(4),
                            Secret = "VerySecretPasswordString"
                        }
                    }
                });

            tokenService = new JwtTokenService(optionsMock.Object);
        }

        [Fact]
        public async Task CreateTokenAsync_Valid_NoExpires()
        {
            // Arrange
            var claims = new Claim[new Random().Next(4, 8)];
            for (int i = 0; i < claims.Length; i++)
            {
                claims[i] = new Claim($"claim{i}", $"{i}value");
            }
            var currentDate = DateTime.UtcNow;
            var expectedExpire = DateTime.UtcNow.Add(optionsMock.Object.Value.Security.Token.LifeTime);

            // Act
            var result = await tokenService.CreateTokenAsync(claims, currentDate, null);
            var decodedResult = new JwtSecurityTokenHandler().ReadJwtToken(result);

            // Assert
            Assert.Contains("testAudience", decodedResult.Audiences);
            Assert.Equal("testIssuer", decodedResult.Issuer);
            Assert.Equal(currentDate, decodedResult.ValidFrom, TimeSpan.FromSeconds(1));
            Assert.Equal(expectedExpire, decodedResult.ValidTo, TimeSpan.FromSeconds(1));
            Assert.Equal((claims.Length + 4), decodedResult.Claims.Count());

            foreach (Claim claim in claims)
            {
                Assert.Contains(claims, x => x == claim);
            }
        }
        [Fact]
        public async Task CreateTokenAsync_Valid_WithExpires()
        {
            // Arrange
            var claims = new Claim[] { new Claim("claim", "value") };
            var expireDate = DateTime.UtcNow.AddHours(1);

            // Act
            var result = await tokenService.CreateTokenAsync(claims, null, expireDate);
            var decodedResult = new JwtSecurityTokenHandler().ReadJwtToken(result);

            // Assert
            Assert.Equal(expireDate, decodedResult.ValidTo, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public async Task CreateTokenAsync_Invalid_RequiresClaims()
        {
            // Arrange
            var claims = new Claim[0];

            // Act & Assert
            var result = await Assert.ThrowsAsync<ArgumentException>(() => tokenService.CreateTokenAsync(claims));
            Assert.Equal("The token must have at least one claim", result.Message);
        }

        [Fact]
        public async Task CreateTokenAsync_Invalid_NoSecretKey()
        {
            // Arrange
            var claims = new Claim[] { new Claim("claim", "value") };

            optionsMock.Setup(x => x.Value)
                .Returns(new AppSettings
                {
                    Security = new SecuritySettings
                    {
                        Token = new JWTTokenSettings
                        {
                            Audience = "testAudience",
                            Issuer = "testIssuer",
                            LifeTime = TimeSpan.FromHours(4)
                        }
                    }
                });

            // Act & Assert
            var result = await Assert.ThrowsAsync<ArgumentException>(() => tokenService.CreateTokenAsync(claims));
            Assert.Equal("Couldn't find the encryption key", result.Message);
        }

        [Fact]
        public async Task CreateTokenAsync_Invalid_NoAudience()
        {
            // Arrange
            var claims = new Claim[] { new Claim("claim", "value") };

            optionsMock.Setup(x => x.Value)
                .Returns(new AppSettings
                {
                    Security = new SecuritySettings
                    {
                        Token = new JWTTokenSettings
                        {
                            Issuer = "testIssuer",
                            LifeTime = TimeSpan.FromHours(4),
                            Secret = "VerySecretPasswordString"
                        }
                    }
                });

            // Act & Assert
            var result = await Assert.ThrowsAsync<ArgumentException>(() => tokenService.CreateTokenAsync(claims));
            Assert.Equal("No token audience is configured!", result.Message);
        }

        [Fact]
        public async Task CreateTokenAsync_Invalid_NoIssuer()
        {
            // Arrange
            var claims = new Claim[] { new Claim("claim", "value") };

            optionsMock.Setup(x => x.Value)
                .Returns(new AppSettings
                {
                    Security = new SecuritySettings
                    {
                        Token = new JWTTokenSettings
                        {
                            Audience = "testAudience",
                            LifeTime = TimeSpan.FromHours(4),
                            Secret = "VerySecretPasswordString"
                        }
                    }
                });

            // Act & Assert
            var result = await Assert.ThrowsAsync<ArgumentException>(() => tokenService.CreateTokenAsync(claims));
            Assert.Equal("No token issuer is configured!", result.Message);
        }
    }
}
