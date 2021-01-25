using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTools.Domain.Data
{
    public class CommentReply
    {
        /// <summary>
        /// Comment identifier
        /// </summary>
        public ulong CommentId { get; set; }
        /// <summary>
        /// Reply identifier
        /// </summary>
        public ulong ReplyId { get; set; }


        /// <summary>
        /// Comment
        /// </summary>
        public virtual Comment Comment { get; set; }
        /// <summary>
        /// Comment reply
        /// </summary>
        public virtual Comment Reply { get; set; }
    }
}
