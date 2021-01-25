using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
        public List<AlbumPicture> Pictures { get; set; }
            = new List<AlbumPicture>();
        public List<AlbumLike> Likes { get; set; }
            = new List<AlbumLike>();


        /// <summary>
        /// Account
        /// </summary>
        public virtual Account Account { get; set; }
    }
}
