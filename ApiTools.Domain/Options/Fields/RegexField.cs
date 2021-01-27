using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ApiTools.Domain.Options.Fields
{
    public class RegexField : ValidationField
    {
        internal Regex RegexObject;

        public string Regex
        {
            get => null;
            set
            {
                RegexObject = new Regex(value);
            }
        }

        public override bool Validate(IList<BadField> badFields, ref string inputString, string fieldName)
        {
            inputString = inputString.Trim();
            if (!RegexObject.IsMatch(inputString))
            {
                badFields.Add(new BadField(fieldName, BadField.Invalid));
                return false;
            }

            return true;
        }
    }
}
