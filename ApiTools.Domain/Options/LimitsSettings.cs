namespace ApiTools.Domain.Options
{
    public class LimitsSettings
    {
        public AccountLimits Account { get; set; }
        public GenericLimits Album { get; set; }
        public GenericLimits Blog { get; set; }
        public GenericLimits BlogPost { get; set; }
        public GenericLimits Event { get; set; }
        public GenericLimits EventCrew { get; set; }
        public GenericLimits EventCrewMember { get; set; }
        public GenericLimits Role { get; set; }
    }
}
