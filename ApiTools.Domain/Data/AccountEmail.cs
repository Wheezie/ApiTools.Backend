namespace ApiTools.Domain.Data
{
    public class AccountEmail
    {
        /// <summary>
        /// Email address (unique, key)
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Account identifier
        /// </summary>
        public ulong? AccountId { get; set; }
        /// <summary>
        /// Address type
        /// </summary>
        public EmailType Type { get; set; }
        /// <summary>
        /// Verified state
        /// </summary>
        public bool Verified { get; set; }

        /// <summary>
        /// Account linked to the email address by the AccountId
        /// </summary>
        public virtual Account Account { get; set; }
    }
}
