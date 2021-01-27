using ApiTools.Domain.Options.Fields;

namespace ApiTools.Domain.Options
{
    public class AccountLimits
    {
        public EmailField Email { get; set; }
        public RequiredField FirstName { get; set; }
        public RequiredField LastName { get; set; }
        public PasswordField Password { get; set; }
        public RequiredField Username { get; set; }
    }
}
