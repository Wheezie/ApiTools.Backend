using System;
using System.Text.Json.Serialization;

namespace ApiTools.Domain.Data.Base
{
    public class Like
    {
        /// <summary>
        /// Liker identifier
        /// </summary>
        public ulong LikerId { get; set; }
        /// <summary>
        /// Action timestamp
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Unliked state
        /// </summary>
        /// <value></value>
        public bool Unliked { get; set; }


        /// <summary>
        /// Account
        /// </summary>
        /// <value></value>
        [JsonIgnore]
        public Account Liker { get; set; }
    }
}
