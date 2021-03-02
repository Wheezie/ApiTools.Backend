using System;

namespace ApiTools.Domain.Options
{
    public class JWTTokenSettings
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public TimeSpan LifeTime { get; set; }
            = TimeSpan.FromDays(1);
        public string Secret { get; set; }
    }
}
