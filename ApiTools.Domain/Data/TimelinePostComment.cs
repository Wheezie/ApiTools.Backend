using ApiTools.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiTools.Domain.Data
{
    public class TimelinePostComment
    {
        /// <summary>
        /// Comment identifier
        /// </summary>
        public ulong CommentId { get; set; }
        /// <summary>
        /// Post identifier
        /// </summary>
        public uint PostId { get; set; }
        /// <summary>
        /// Post account identifier
        /// </summary>
        public ulong AccountId { get; set; }

        /// <summary>
        /// Post comment reference
        /// </summary>
        public virtual Comment Comment { get; set; }
        /// <summary>
        /// Post
        /// </summary>
        public virtual TimelinePost Post { get; set; }
        /// <summary>
        /// Poster
        /// </summary>
        public virtual Account Account { get; set; }
    }
}
