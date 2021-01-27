using System.Collections.Generic;

namespace ApiTools.Domain.Data
{
    public class Album
    {
        /// <summary>
        /// Album identifier
        /// </summary>
        public uint Id { get; set; }
        /// <summary>
        /// Account identifier
        /// </summary>
        public ulong? AccountId { get; set; }
        /// <summary>
        /// Album name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Album description
        /// </summary>
        public string Description { get; set; }
        public ICollection<Picture> Pictures { get; set; }
        public ICollection<AlbumLike> Likes { get; set; }


        /// <summary>
        /// Account
        /// </summary>
        public virtual Account Account { get; set; }
    }
}
