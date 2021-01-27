using ApiTools.Domain.Data.Base;
using System.Collections.Generic;

namespace ApiTools.Domain.Data
{
    public class TimelinePost : Post
    {
        /// <summary>
        /// Pictures
        /// </summary>
        public ICollection<Picture> Pictures { get; set; }
        /// <summary>
        /// Comments
        /// </summary>
        public ICollection<Comment> Comments { get; set; }
        /// <summary>
        /// Likes
        /// </summary>
        public ICollection<TimelinePostLike> Likes { get; set; }
    }
}
