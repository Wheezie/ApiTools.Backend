using ApiTools.Domain.Data.Base;
using System;
using System.Text.Json.Serialization;

namespace ApiTools.Domain.Data
{
    public class PictureLike : Like
    {
        public Guid PictureId { get; set; }

        [JsonIgnore]
        public virtual Picture Picture { get; set; }
    }
}
