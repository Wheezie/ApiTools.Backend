using ApiTools.Domain.Data.Base;

namespace ApiTools.Domain.Data
{
    public class BlogPostLike : Like
    {
        /// <summary>
        /// Post identifier
        /// </summary>
        public uint PostId { get; set; }


        /// <summary>
        /// Post
        /// </summary>
        public virtual BlogPost Post { get; set; }
    }
}