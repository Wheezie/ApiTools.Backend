namespace ApiTools.Domain
{
    /// <summary>
    /// A BadField error
    /// </summary>
    public partial class BadField
    {
        public const string AlreadyExists = "already_exists";
        public const string Required = "required";
        public const string Invalid = "invalid";
        public const string NotConfirmed = "not_confirmed";
        public const string NotFound = "not_found";
        public const string ToLong = "to_long";
        public const string ToShort = "to_short";
        public const string RequiresDigits = "requires_digits";
        public const string RequiresSpecials = "requires_specials";

        /// <summary>
        /// Field that is incorrectly provided
        /// </summary>
        public string Field { get; set; }
        /// <summary>
        /// Explanation for the input error
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// Create a BadField instance
        /// </summary>
        /// <param name="field">incorrect field name</param>
        /// <param name="error">error type</param>
        public BadField(string field, string error)
        {
            Field = field;
            Error = error;
        }
    }
}