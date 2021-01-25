using ApiTools.Domain.Data.Base;
using System.Collections.Generic;

namespace ApiTools.Domain.Data
{
    public class TimelinePost : Post
    {
        /// <summary>
        /// Pictures
        /// </summary>
        public List<TimelinePostPicture> Pictures { get; set; }
            = new List<TimelinePostPicture>();
        /// <summary>
        /// Comments
        /// </summary>
        public List<TimelinePostComment> Comments { get; set; }
            = new List<TimelinePostComment>();
        /// <summary>
        /// Likes
        /// </summary>
        public List<TimelinePostLike> Likes { get; set; }
            = new List<TimelinePostLike>();
    }
}
