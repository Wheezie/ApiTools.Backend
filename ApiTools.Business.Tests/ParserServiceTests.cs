using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ApiTools.Business.Tests
{
    public class ParserServiceTests
    {
        private ParserService parserService;
        private const string TextTemplate = "<!DOCTYPE html><html><head><meta charset=\"utf-8\"/><title>Hello {{TO_REPLACE}}</title></head><body><header>Welcome {{TO_REPLACE2}}{{NOT_REPLACED}}</header><span>This is a test email template</span></body></html>";

        public ParserServiceTests()
        {
            parserService = new ParserService();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task ParseHtmlFromTemplateAsync_WithPath_EmptyFileName(string path)
        {
            // Act
            await Assert.ThrowsAnyAsync<ArgumentException>(() => parserService.ParseFromTemplateAsync(path, null));
        }


        [Theory]
        [InlineData("C:\\invalid\\path")]
        [InlineData("/invalid/path")]
        [InlineData("nonexistant_file.html")]
        public async Task ParseHtmlFromTemplateAsync_WithPath_InvalidFileName(string path)
        {
            // Act
            await Assert.ThrowsAnyAsync<IOException>(() => parserService.ParseFromTemplateAsync(path, null));
        }

        [Fact]
        public async Task ParseHtmlFromTemplateAsync_WithPath_Valid()
        {
            // Arrange
            string path = "./assets/parser_test.html";
            Dictionary<string, string> kvPairs = new Dictionary<string, string>
            {
                { "USER_NAME", "FilledUsername" },
                { "FIRST_NAME", "FirstName" },
                { "LAST_NAME", "LastName" },
            };

            // Act
            string result = await parserService.ParseFromTemplateAsync(path, kvPairs);

            // Assert
            foreach (var pair in kvPairs)
            {
                Assert.DoesNotContain($"{{{pair.Key}}}", result);
                Assert.Contains(pair.Value, result);
            }
        }

        [Fact]
        public async Task ParseHtmlFromTemplateAsync_WithReader_Valid()
        {
            // Arrange
            Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(TextTemplate));
            StreamReader reader = new StreamReader(stream);
            Dictionary<string, string> kvPairs = new Dictionary<string, string>
            {
                { "TO_REPLACE", "Replaced" },
                { "TO_REPLACE2", "Replaced" },
            };

            // Act
            string result = await parserService.ParseFromTemplateAsync(reader, kvPairs);

            // Assert
            foreach (var pair in kvPairs)
            {
                Assert.DoesNotContain($"{{{pair.Key}}}", result);
                Assert.Contains(pair.Value, result);
            }

            Assert.DoesNotContain("NotReplaced", result);
        }
    }
}
