using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTools.Domain.Data
{
    public class TimelinePostPicture
    {
        /// <summary>
        /// Post identifier
        /// </summary>
        /// <value></value>
        public uint PostId { get; set; }
        /// <summary>
        /// Account identifier
        /// </summary>
        /// <value></value>
        public ulong AccountId { get; set; }
        /// <summary>
        /// Picture identifier
        /// </summary>
        public Guid PictureId { get; set; }


        /// <summary>
        /// Account post
        /// </summary>
        public virtual TimelinePost Post { get; set; }
        /// <summary>
        /// Picture
        /// </summary>
        public virtual Picture Picture { get; set; }
        /// <summary>
        /// Account
        /// </summary>
        public virtual Account Account { get; set; }
    }
}
