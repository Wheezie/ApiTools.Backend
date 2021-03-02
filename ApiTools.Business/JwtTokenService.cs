using ApiTools.Business.Contracts;
using ApiTools.Domain;
using ApiTools.Domain.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApiTools.Business
{
    internal class JwtTokenService : ITokenService
    {
        private readonly IOptions<AppSettings> options;
        private readonly JwtSecurityTokenHandler tokenHandler;

        public JwtTokenService(IOptions<AppSettings> options)
        {
            this.options = options;
            tokenHandler = new JwtSecurityTokenHandler();
        }

        public Task<string> CreateTokenAsync(IEnumerable<Claim> claims, DateTime? notBeforeDate = null, DateTime? expires = null)
        {
            if (claims.Count() < 1)
            {
                throw new ArgumentException("The token must have at least one claim");
            }

            return Task.Run(() =>
            {
                JWTTokenSettings settings = options.Value.Security.Token;
                if (string.IsNullOrWhiteSpace(settings.Secret))
                {
                    throw new ArgumentException("Couldn't find the encryption key");
                }

                if (string.IsNullOrWhiteSpace(settings.Audience))
                {
                    throw new ArgumentException("No token audience is configured!");
                }
                
                if (string.IsNullOrWhiteSpace(settings.Issuer))
                {
                    throw new ArgumentException("No token issuer is configured!");
                }

                SecurityKey securityKey
                    = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Secret));

                JwtSecurityToken token = new JwtSecurityToken(
                    audience: settings.Audience,
                    issuer: settings.Issuer,
                    notBefore: notBeforeDate,
                    expires: expires ?? DateTime.UtcNow.Add(settings.LifeTime),
                    claims: claims,
                    signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512)
                );

                return tokenHandler.WriteToken(token);
            });
        }
    }
}
