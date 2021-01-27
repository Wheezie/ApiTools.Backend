using ApiTools.Domain.Enum;
using System;
using System.Collections.Generic;

namespace ApiTools.Domain.Data
{
    public class Blog
    {
        /// <summary>
        /// Blog identifier
        /// </summary>
        public uint Id { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Descriptive name
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Creator's account identifier
        /// </summary>
        public ulong? CreatorId { get; set; }
        /// <summary>
        /// Created date
        /// </summary>
        public DateTime Created { get; set; }
        /// <summary>
        /// Visibility state
        /// </summary>
        public Visibility Visibility { get; set; }
        /// <summary>
        /// User access grants
        /// </summary>
        public ICollection<BlogUserAccess> UserAccess { get; set; }
        /// <summary>
        /// Role access grants
        /// </summary>
        public ICollection<BlogRoleAccess> RoleAccess { get; set; }
        /// <summary>
        /// Associated posts
        /// </summary>
        public ICollection<BlogPost> Posts { get; set; }


        /// <summary>
        /// Blog creator
        /// </summary>
        /// <value></value>
        public virtual Account Creator { get; set; }
    }
}