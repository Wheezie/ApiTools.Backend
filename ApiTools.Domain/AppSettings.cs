using ApiTools.Domain.Options;

namespace ApiTools.Domain
{
    public class AppSettings
    {
        public LimitsSettings Limits { get; set; }
        public SmtpSettings Mail { get; set; }
        public SecuritySettings Security { get; set; }
        public TemplateSettings Templates { get; set; }
        public EndpointSettings Endpoints { get; set; }
    }
}
