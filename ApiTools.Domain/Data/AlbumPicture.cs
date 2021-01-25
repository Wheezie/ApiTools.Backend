using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTools.Domain.Data
{
    public class AlbumPicture
    {
        /// <summary>
        /// Album identifier
        /// </summary>
        public uint Id { get; set; }
        /// <summary>
        /// Picture identifier
        /// </summary>
        public Guid PictureId { get; set; }


        /// <summary>
        /// Album
        /// </summary>
        public Album Album { get; set; }
        /// <summary>
        /// Picture
        /// </summary>
        public Picture Picture { get; set; }
    }
}
