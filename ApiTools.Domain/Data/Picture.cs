using ApiTools.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiTools.Domain.Data
{
    public class Picture
    {
        /// <summary>
        /// Picture identifier
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Uploaded timestamp
        /// </summary>
        public DateTime Uploaded { get; set; }
        /// <summary>
        /// Uploader identifier
        /// </summary>
        public ulong? UploaderId { get; set; }
        /// <summary>
        /// Current state
        /// </summary>
        public Visibility State { get; set; }
        /// <summary>
        /// State changed timestamp
        /// </summary>
        public DateTime? StateChanged { get; set; }
        /// <summary>
        /// Account likes.
        /// </summary>
        public List<PictureLike> Likes { get; set; }


        /// <summary>
        /// Uploader
        /// </summary>
        [JsonIgnore]
        public virtual Account Uploader { get; set; }
    }
}
