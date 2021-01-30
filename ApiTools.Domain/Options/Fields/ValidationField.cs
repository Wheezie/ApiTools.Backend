using System.Collections.Generic;

namespace ApiTools.Domain.Options.Fields
{
    public class ValidationField
    {
        private int minimum;
        private int maximum;

        protected string fieldName;

        public int Minimum
        {
            get => minimum;
            set
            {
                if (minimum != value)
                {
                    minimum = value;

                    RectifyValues();
                }
            }
        }
        public int Maximum
        {
            get => maximum;
            set
            {
                if (maximum != value)
                {
                    maximum = value;

                    RectifyValues();
                }
            }
        }

        public virtual bool Validate(IList<BadField> badFields, string inputString, string fieldName)
        {
            inputString = inputString.Trim();
            if (inputString.Length < minimum)
            {
                badFields.Add(new BadField(fieldName, BadField.ToShort));
            }
            else if (inputString.Length > maximum)
            {
                badFields.Add(new BadField(fieldName, BadField.ToLong));
            }
            else
            {
                return true;
            }

            return false;
        }

        private void RectifyValues()
        {
            // Switch around variables if they are in the incorrect order.
            if (minimum > maximum)
            {
                maximum += minimum;
                minimum = maximum - minimum;
                maximum -= minimum;
            }
        }
    }
}
