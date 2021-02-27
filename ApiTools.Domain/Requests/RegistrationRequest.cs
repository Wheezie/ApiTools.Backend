using ApiTools.Domain.Options;
using System.Collections.Generic;

namespace ApiTools.Domain.Requests
{
    public class RegistrationRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }

        public IReadOnlyList<BadField> Validate(AccountLimits config)
        {
            List<BadField> badFields = new List<BadField>();
            config.Username.Validate(badFields, Username, nameof(Username));
            config.Email.Validate(badFields, Email, nameof(Email));
            config.FirstName.Validate(badFields, FirstName, nameof(FirstName));
            config.LastName.Validate(badFields, LastName, nameof(LastName));
            config.Password.Validate(badFields, Password, nameof(Password));

            return badFields;
        }
    }
}