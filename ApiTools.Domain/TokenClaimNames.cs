using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTools.Domain
{
    public struct TokenClaimNames
    {
        public const string AccountId = "sid";
        public const string ConcurrencyStamp = "nonce";
        public const string Role = "role";
        public const string Username = "sub";
        public const string Token = "jti";
    }
}
