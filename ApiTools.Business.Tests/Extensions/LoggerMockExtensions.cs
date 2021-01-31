using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTools.Business.Tests
{
    public static class LoggerMockExtensions
    {
        public static void CatchLog<T>(this Mock<ILogger<T>> mock, string containsMessage, LogLevel level = LogLevel.Information, Times times = default(Times))
        {
            mock.Verify(
                x => x.Log(level, It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>(
                        (v, _) => v.ToString().Contains(containsMessage)),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                times);
        }
        public static void CatchExceptionLog<T, T2>(this Mock<ILogger<T>> mock, string containsMessage, LogLevel level = LogLevel.Information, Times times = default(Times)) where T2 : Exception
        {
            mock.Verify(
                x => x.Log(level, It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>(
                        (v, _) => v.ToString().Contains(containsMessage)),
                    It.IsAny<T2>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                times);
        }
    }
}
