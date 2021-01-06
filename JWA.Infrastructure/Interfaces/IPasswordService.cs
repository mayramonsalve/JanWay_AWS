using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWA.Infrastructure.Interfaces
{
    public interface IPasswordService
    {
        string Hash(string password);

        bool Check(string hash, string password);
        string CreateAccessToken(IEnumerable<Claim> claims, TimeSpan? expiration = null);
        List<Claim> CreateJwtClaims(ClaimsIdentity identity);
        JwtSecurityToken ReadToken(string token);
    }
}
