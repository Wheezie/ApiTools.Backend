namespace ApiTools.Domain.Data
{
    public class BlogPostComment
    {
        /// <summary>
        /// Blog identifier
        /// </summary>
        /// <value></value>
        public uint BlogId { get; set; }
        /// <summary>
        /// Post identifier
        /// </summary>
        public uint PostId { get; set; }
        public ulong CommentId { get; set; }


        /// <summary>
        /// Blog
        /// </summary>
        public virtual Blog Blog { get; set; }
        /// <summary>
        /// Post
        /// </summary>
        public virtual BlogPost Post { get; set; }
        /// <summary>
        /// Comment
        /// </summary>
        public virtual Comment Comment { get; set; }
    }
}