using System.Collections.Generic;

namespace ApiTools.Domain.Options.Fields
{
    public class RequiredField : ValidationField
    {
        public bool Required { get; set; }
            = true;

        public override bool Validate(IList<BadField> badFields, ref string inputString, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(inputString) && Required)
            {
                return false;
            }

            return base.Validate(badFields, ref inputString, fieldName);
        }
    }
}
