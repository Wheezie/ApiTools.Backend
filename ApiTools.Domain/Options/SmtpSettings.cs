namespace ApiTools.Domain.Options
{
    public class SmtpSettings
    {
        public MailCredential Credentials { get; set; }
        public string Hostname { get; set;  }
        public int Port { get; set; }
        public bool Ssl { get; set; }
    }
}
