using ApiTools.Domain.Data.Base;

namespace ApiTools.Domain.Data
{
    public class AlbumLike : Like
    {
        /// <summary>
        /// Album identifier
        /// </summary>
        public uint AlbumId { get; set; }

        /// <summary>
        /// Album
        /// </summary>
        public virtual Album Album { get; set; }
    }
}
