using ApiTools.Domain.Data.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
