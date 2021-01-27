using System.Text.RegularExpressions;

namespace ApiTools.Domain.Options.Fields
{
    public class EmailField : RegexField
    {
        public EmailField()
        {
            RegexObject = new Regex(Regexes.Email);
        }
    }
}