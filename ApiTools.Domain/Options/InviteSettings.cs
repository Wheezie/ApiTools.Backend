using System;

namespace ApiTools.Domain.Options
{
    public class InviteSettings : EnabledSettings
    {
        public TimeSpan Lifetime { get; set; }
    }
}
