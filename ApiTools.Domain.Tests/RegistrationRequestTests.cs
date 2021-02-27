using ApiTools.Domain.Options;
using ApiTools.Domain.Options.Fields;
using ApiTools.Domain.Requests;
using System;
using System.Linq;
using Xunit;

namespace ApiTools.Domain.Tests
{
    public class RegistrationRequestTests
    {
        private readonly AccountLimits limits;

        public RegistrationRequestTests()
        {
            limits = new AccountLimits
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
            };
        }

        [Fact]
        public void Validate_Failed_Trim()
        {
            // Arrange
            string trimmedString = "                 1              ";
            var regRequest = new RegistrationRequest
            {
                Username = trimmedString,
                FirstName = trimmedString,
                LastName = trimmedString,
                Email = "valid@domain.tld",
                Password = "P@ssw0rd123"
            };

            // Act
            var badFields = regRequest.Validate(limits);

            // Assert
            Assert.NotEmpty(badFields);
            foreach (var field in badFields)
            {
                Assert.Equal(BadField.ToShort, field.Error);
            }
        }

        [Fact]
        public void Validate_Failed_ToShort()
        {
            // Arrange
            string trimmedString = "1";
            var regRequest = new RegistrationRequest
            {
                Username = trimmedString,
                FirstName = trimmedString,
                LastName = trimmedString,
                Email = "valid@domain.tld",
                Password = "P@ssw0rd123"
            };

            // Act
            var badFields = regRequest.Validate(limits);

            // Assert
            Assert.NotEmpty(badFields);
            foreach (var field in badFields)
            {
                Assert.Equal(BadField.ToShort, field.Error);
            }
        }
    }
}
