using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTools.Domain.Data
{
    /// <summary>
    /// Administrative & permissive role
    /// </summary>
    public class Role : IdentityRole<ulong>
    {
        /// <summary>
        /// Role description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Disabled state, if enabled role will be unavailable & defaulted.
        /// </summary>
        public bool Disabled { get; set; }
        /// <summary>
        /// Targetable Role identifier
        /// </summary>
        public ulong Targets { get; set; }
        /// <summary>
        /// Permissions
        /// </summary>
        public List<RolePermission> Permissions { get; set; }
            = new List<RolePermission>();
        /// <summary>
        /// Members
        /// </summary>
        /// <value></value>
        public List<Account> Members { get; set; }
            = new List<Account>();

        /// <summary>
        /// Blog role access
        /// </summary>
        public List<BlogRoleAccess> BlogAccess { get; set; }
            = new List<BlogRoleAccess>();
    }
}
