using ApiTools.Domain.Enum;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace ApiTools.Domain.Data
{
    public class Account : IdentityUser<ulong>
    {
        /// <summary>
        /// First name
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Last name
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Full account name
        /// </summary>
        public string FullName
            => $"{this.FirstName} {this.LastName}";
        /// <summary>
        /// Description (may be used for administrative purposes)
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Public bio
        /// </summary>
        public string Bio { get; set; }
        /// <summary>
        /// Website
        /// </summary>
        public string Website { get; set; }
        /// <summary>
        /// Email address
        /// </summary>
        public string ProfileEmailId { get; set; }
        /// <summary>
        /// Name visibility
        /// </summary>
        public Visibility ShowName { get; set; }
        /// <summary>
        /// Email visibility
        /// </summary>
        public Visibility ShowEmail { get; set; }
        /// <summary>
        /// Website visibility
        /// </summary>
        public Visibility ShowWebsite { get; set; }
        /// <summary>
        /// Account picture (if any)
        /// </summary>
        public Guid? PictureId { get; set; }
        /// <summary>
        /// Registration date
        /// </summary>
        public DateTime RegisteredDate { get; set; }
        /// <summary>
        /// Blocked datestamp
        /// </summary>
        public DateTime? BlockedDate { get; set; }
        public bool Confirmed
            => EmailConfirmed; // || PhoneConfirmed; // Either email or phone number must be confirmed

        public ulong RoleId { get; set; }
        /// <summary>
        /// Corresponding email address list
        /// </summary>
        public List<AccountEmail> Emails { get; set; }
            = new List<AccountEmail>();
        /// <summary>
        /// Created invite tokens
        /// </summary>
        public List<AccountInvite> Invites { get; set; }
            = new List<AccountInvite>();
        /// <summary>
        /// Login JWT sessions
        /// </summary>
        public List<JwtSession> Sessions { get; set; }

        /// <summary>
        /// Account invite
        /// </summary>
        public virtual AccountInvite Invite { get; set; }
        /// <summary>
        /// Profile email
        /// </summary>
        public virtual AccountEmail ProfileEmail { get; set; }
        /// <summary>
        /// Primary account email
        /// </summary>
        public virtual AccountEmail PrimaryEmail { get; set; }
        /// <summary>
        /// Account picture
        /// </summary>
        public virtual Picture Picture { get; set; }
        /// <summary>
        /// Primary role
        /// </summary>
        public virtual Role Role { get; set; }

        public override string NormalizedEmail { get => Email.ToUpper(); }
    }
}
