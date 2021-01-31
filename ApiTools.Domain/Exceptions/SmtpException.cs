using System;

namespace ApiTools.Domain.Exceptions
{
    public class SmtpException : ApiToolsException
    {
        public SmtpException(string message, Exception innerException) : base(message, innerException, true)
        {
        }
    }
}
