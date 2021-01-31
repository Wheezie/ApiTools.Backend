using System;

namespace ApiTools.Domain
{
    public abstract class ApiToolsException : Exception
    {
        public bool Logged { get; private init; }

        protected ApiToolsException(bool logged = false) : this(null, null, logged)
        {
        }

        protected ApiToolsException(string message, bool logged = false) : this(message, null, logged)
        {
        }

        protected ApiToolsException(string message, Exception innerException, bool logged = false) : base(message, innerException)
        {
            Logged = logged;
        }
    }
}
