using ApiTools.Domain.Enum;

namespace ApiTools.Domain.Data.Base
{
    public class RoleAccess
    {
        /// <summary>
        /// Role identifier
        /// </summary>
        public ulong RoleId { get; set; }
        /// <summary>
        /// Access list
        /// </summary>
        public Access Access { get; set; }

        /// <summary>
        /// Role
        /// </summary>
        public Role Role { get; set; }
    }
}
