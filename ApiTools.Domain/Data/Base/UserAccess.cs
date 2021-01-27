using ApiTools.Domain.Enum;

namespace ApiTools.Domain.Data.Base
{
    public class UserAccess
    {
        /// <summary>
        /// Account identifier
        /// </summary>
        public ulong AccountId { get; set; }
        /// <summary>
        /// Access list
        /// </summary>
        public Access Access { get; set; }

        /// <summary>
        /// Account
        /// </summary>
        public Account Account { get; set; }
    }
}
