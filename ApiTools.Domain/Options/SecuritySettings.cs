namespace ApiTools.Domain.Options
{
    public class SecuritySettings
    {
        public int DefaultRole { get; set; }
        public InviteSettings Invite { get; set; }
        public bool KeepEMailBanned { get; set; }
        public object Password { get; set; }
        public EnabledSettings Registration { get; set; }
        public SessionSettings Session { get; set; }
        public bool SetupDefaultAdmin { get; set; }
        public JWTTokenSettings Token { get; set; }
    }

}
