using System;

namespace ApiTools.Domain.Options
{
    public class SessionSettings
    {
        public bool LogLastIp { get; set; }
        public TimeSpan Expires { get; set; }
    }
}
