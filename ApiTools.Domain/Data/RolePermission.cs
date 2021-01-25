using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTools.Domain.Data
{
    public class RolePermission
    {
        /// <summary>
        /// Permission role identifier
        /// </summary>
        public ulong RoleId { get; set; }
        /// <summary>
        /// Permission targetable role identifier
        /// </summary>
        public ulong? Target { get; set; }
        /// <summary>
        /// Permission
        /// </summary>
        public string Permission { get; set; }


        /// <summary>
        /// Permission role
        /// </summary>
        public virtual Role Role { get; set; }
    }
}
