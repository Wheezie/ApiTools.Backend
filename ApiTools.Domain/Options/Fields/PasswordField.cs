using System.Collections.Generic;
using System.Linq;

namespace ApiTools.Domain.Options.Fields
{
    public class PasswordField : RequiredField
    {
        public bool Number { get; set; }
        public bool SpecialCharacter { get; set; }

        public override bool Validate(IList<BadField> badFields, string inputString, string fieldName)
        {
            bool result = base.Validate(badFields, inputString, fieldName);
            if (Number && !inputString.Any(c => char.IsDigit(c)))
            {
                badFields.Add(new BadField(fieldName, BadField.RequiresDigits));
                result = false;
            }

            if (SpecialCharacter && !inputString.Any(c => !char.IsLetterOrDigit(c)))
            {
                badFields.Add(new BadField(fieldName, BadField.RequiresSpecials));
                result = false;
            }

            return result;
        }
    }
}
