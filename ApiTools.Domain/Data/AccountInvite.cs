using System;

namespace ApiTools.Domain.Data
{
    public class AccountInvite
    {
        /// <summary>
        /// Token identifier
        /// </summary>
        public Guid Token { get; set; }
        /// <summary>
        /// Inviter identifier
        /// </summary>
        /// <value></value>
        public ulong? InviterId { get; set; }
        /// <summary>
        /// Invited date
        /// </summary>
        public DateTime Invited { get; set; }
        /// <summary>
        /// Token expiration date
        /// </summary>
        public DateTime Expire { get; set; }
        /// <summary>
        /// Accepting account identifier
        /// </summary>
        public ulong? AcceptorId { get; set; }


        /// <summary>
        /// Invited account
        /// </summary>
        public virtual Account Inviter { get; set; }
        /// <summary>
        /// Accepting account
        /// </summary>
        public virtual Account Acceptor { get; set; }
    }
}
