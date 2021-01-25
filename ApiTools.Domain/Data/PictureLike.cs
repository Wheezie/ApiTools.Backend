using System;
using System.Text.Json.Serialization;

namespace ApiTools.Domain.Data
{
    public class PictureLike
    {
        public Guid PictureId { get; set; }
        public ulong AccountId { get; set; }
        public DateTime Date { get; set; }
        public bool Unliked { get; set; }

        [JsonIgnore]
        public virtual Picture Picture { get; set; }
        [JsonIgnore]
        public virtual Account Account { get; set; }
    }
}
