using ApiTools.Domain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApiTools.Business.Contracts
{
    public interface ITokenService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="notBeforeDate"></param>
        /// <param name="expires"></param>
        /// <returns></returns>
        Task<string> CreateTokenAsync(IEnumerable<Claim> claims, DateTime? notBeforeDate = null, DateTime? expires = null);
    }
}
