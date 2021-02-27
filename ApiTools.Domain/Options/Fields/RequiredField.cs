using System.Collections.Generic;

namespace ApiTools.Domain.Options.Fields
{
    public class RequiredField : ValidationField
    {
        public bool Required { get; set; }
            = true;

        public override bool Validate(IList<BadField> badFields, string inputString, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(inputString) && Required)
            {
                badFields.Add(new BadField(fieldName, BadField.Required));
                return false;
            }

            return base.Validate(badFields, inputString, fieldName);
        }
    }
}
