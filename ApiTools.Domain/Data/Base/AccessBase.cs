using ApiTools.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTools.Domain.Data.Base
{
    public class AccessBase
    {
        /// <summary>
        /// Account identifier
        /// </summary>
        public ulong AccountId { get; set; }
        /// <summary>
        /// Role identifier
        /// </summary>
        public ulong RoleId { get; set; }
        /// <summary>
        /// Access list
        /// </summary>
        public Access Access { get; set; }

        /// <summary>
        /// Account
        /// </summary>
        public Account Account { get; set; }
        /// <summary>
        /// Role
        /// </summary>
        public Role Role { get; set; }
    }
}
