namespace ApiTools.Domain.Options
{
    public class PasswordSettings
    {
        public int Memory { get; set; }
        public int Threads { get; set; }
        public int Factor { get; set; }
        public int HashLength { get; set; }
    }
}
