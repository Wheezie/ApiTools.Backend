using System;

namespace ApiTools.Domain.Data
{
    public class JwtSession
    {
        /// <summary>
        /// Session token
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Account identifier
        /// </summary>
        public ulong? AccountId { get; set; }
        /// <summary>
        /// Browser type
        /// </summary>
        public byte Browser { get; set; }
        /// <summary>
        /// Platform type
        /// </summary>
        public byte Platform { get; set; }
        /// <summary>
        /// Session issued date
        /// </summary>
        public DateTime Issued { get; set; }
        /// <summary>
        /// Last token use
        /// </summary>
        public DateTime? LastUse { get; set; }
        /// <summary>
        /// Expire
        /// </summary>
        public DateTime? Expires { get; set; }
        /// <summary>
        /// Issued ip address
        /// </summary>
        public byte[] Ip { get; set; }
        /// <summary>
        /// Last ip address
        /// </summary>
        public byte[] LastIp { get; set; }


        /// <summary>
        /// Account
        /// </summary>
        public virtual Account Account { get; set; }
    }
}
