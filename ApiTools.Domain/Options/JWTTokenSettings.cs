namespace ApiTools.Domain.Options
{
    public class JWTTokenSettings
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int LifeTime { get; set; }
        public string Secret { get; set; }
    }
}
