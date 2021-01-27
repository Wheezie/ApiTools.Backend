using System.Collections.Generic;

namespace ApiTools.Domain.Options.Fields
{
    public class ValidationField
    {
        private int _minimum;
        private int _maximum;

        public int Minimum
        {
            get => _minimum;
            set
            {
                if (_minimum != value)
                {
                    _minimum = value;

                    RectifyValues();
                }
            }
        }
        public int Maximum
        {
            get => _maximum;
            set
            {
                if (_maximum != value)
                {
                    _maximum = value;

                    RectifyValues();
                }
            }
        }

        public virtual bool Validate(IList<BadField> badFields, ref string inputString, string fieldName)
        {
            inputString = inputString.Trim();
            if (inputString.Length < _minimum)
            {
                badFields.Add(new BadField(fieldName, BadField.ToShort));
            }
            else if (inputString.Length > _maximum)
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
            if (_minimum > _maximum)
            {
                _maximum += _minimum;
                _minimum = _maximum - _minimum;
                _maximum -= _minimum;
            }
        }
    }
}
