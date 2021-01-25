using System;

namespace ApiTools.Domain.Data.Base
{
    public class ContentBase<T>
    {
        /// <summary>
        /// Content Identifier
        /// </summary>
        public T Id { get; set; }
        /// <summary>
        /// Account identifier
        /// </summary>
        public ulong? AccountId { get; set; }
        /// <summary>
        /// Content (parsed)
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// Created timestamp
        /// </summary>
        public DateTime Created { get; set; }
        /// <summary>
        /// Modified timestamp
        /// </summary>
        public DateTime? Modified { get; set; }

        /// <summary>
        /// Account
        /// </summary>
        public virtual Account Account { get; set; }
    }
}
