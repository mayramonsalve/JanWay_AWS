﻿using JWA.Infrastructure.Interfaces;
using JWA.Infrastructure.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace JWA.Infrastructure.Services
{
    public class PasswordService : IPasswordService
    {
        public readonly PasswordOptions _options;
        public PasswordService(IOptions<PasswordOptions> options)
        {
            _options = options.Value;
        }
        bool IPasswordService.Check(string hash, string password)
        {
            var parts = hash.Split(".");

            if(parts.Length != 3)
            {
                throw new FormatException("Unexpected hash format.");
            }

            var iterations = Convert.ToInt32(parts[0]);
            var salt = Convert.FromBase64String(parts[1]);
            var key = Convert.FromBase64String(parts[2]);

            using (var algorithm = new Rfc2898DeriveBytes(password, salt, iterations))
            {
                var keyToCheck = algorithm.GetBytes(_options.KeySize);
                return keyToCheck.SequenceEqual(key);
            }
        }

        string IPasswordService.Hash(string password)
        {
            //FBKDF2 Implementation
            using (var algorithm = new Rfc2898DeriveBytes(password, _options.SaltSize, _options.Iterations))
            {
                var key = Convert.ToBase64String(algorithm.GetBytes(_options.KeySize));
                var salt = Convert.ToBase64String(algorithm.Salt);
                return $"{_options.Iterations}.{salt}.{key}";
            }
        }
        string IPasswordService.CreateAccessToken(IEnumerable<Claim> claims, TimeSpan? expiration = null)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("JWA.Auth_M@yr@Jh0@n@J3r3my2020"));   //get by appsetting
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var now = DateTime.UtcNow;
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: "Jan-Way",      //get by appsetting
                audience: "Jan-Way",      //get by appsetting
                claims: claims,
                notBefore: now,
                expires: DateTime.UtcNow.AddMonths(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }
        List<Claim> IPasswordService.CreateJwtClaims(ClaimsIdentity identity)
        {
            var claims = identity.Claims.ToList();
            var nameIdClaim = claims.First(c => c.Type == ClaimTypes.NameIdentifier);

            // Specifically add the jti (random nonce), iat (issued timestamp), and sub (subject/user) claims.
            claims.AddRange(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, nameIdClaim.Value),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToString(), ClaimValueTypes.Integer64)
            });

            return claims;
        }
        JwtSecurityToken IPasswordService.ReadToken(string token)
        {
            var _token = new JwtSecurityTokenHandler();
            var TokenS = _token.ReadJwtToken(token) as JwtSecurityToken;
            return TokenS;
        }
    }
}
