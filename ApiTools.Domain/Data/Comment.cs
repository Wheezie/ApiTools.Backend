using ApiTools.Domain.Data.Base;
using System.Collections.Generic;

namespace ApiTools.Domain.Data
{
    public class Comment : ContentBase<ulong>
    {
        /// <summary>
        /// Replies
        /// </summary>
        public List<CommentReply> Replies { get; set; }
    }
}
