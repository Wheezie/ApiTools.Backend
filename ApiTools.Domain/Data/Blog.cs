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
        /// Access grants
        /// </summary>
        public List<BlogAccess> Access { get; set; }
            = new List<BlogAccess>();
        /// <summary>
        /// Associated posts
        /// </summary>
        public List<BlogPost> Posts { get; set; }
            = new List<BlogPost>();


        /// <summary>
        /// Blog creator
        /// </summary>
        /// <value></value>
        public virtual Account Creator { get; set; }
    }
}