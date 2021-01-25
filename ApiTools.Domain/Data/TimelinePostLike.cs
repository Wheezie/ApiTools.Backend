using ApiTools.Domain.Data.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTools.Domain.Data
{
    public class TimelinePostLike : Like
    {
        /// <summary>
        /// Account identifier
        /// </summary>
        public ulong AccountId { get; set; }
        /// <summary>
        /// Post identifier
        /// </summary>
        public uint PostId { get; set; }



        /// <summary>
        /// Account
        /// </summary>
        public virtual Account Account { get; set; }
        /// <summary>
        /// Post
        /// </summary>
        public virtual TimelinePost Post { get; set; }
    }
}
