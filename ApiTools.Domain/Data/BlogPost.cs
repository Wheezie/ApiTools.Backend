using ApiTools.Domain.Data.Base;
using System.Collections.Generic;

namespace ApiTools.Domain.Data
{
    public class BlogPost : Post
    {
        /// <summary>
        /// Blog identifier
        /// </summary>
        public uint BlogId { get; set; }
        /// <summary>
        /// Comments
        /// </summary>
        public ICollection<Comment> Comments { get; set; }


        /// <summary>
        /// Blog
        /// </summary>
        /// <value></value>
        public virtual Blog Blog { get; set;}
    }
}