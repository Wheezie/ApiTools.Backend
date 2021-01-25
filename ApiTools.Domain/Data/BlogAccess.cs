using ApiTools.Domain.Data.Base;

namespace ApiTools.Domain.Data
{
    public class BlogAccess : AccessBase
    {
        /// <summary>
        /// Blog identifier
        /// </summary>
        public uint BlogId { get; set; }


        /// <summary>
        /// Blog
        /// </summary>
        public virtual Blog Blog { get; set; }
    }
}