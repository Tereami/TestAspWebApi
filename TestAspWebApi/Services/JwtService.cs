using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestAspWebApi.Models;

namespace TestAspWebApi.Services
{
    public class JwtService
    {
        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(ApplicationUser user)
        {
            var jwt = _config.GetSection("Jwt");
            var keyBytes = Encoding.UTF8.GetBytes(jwt["Key"]!);
            var key = new SymmetricSecurityKey(keyBytes);

            Claim[] claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
                new Claim(ClaimTypes.Name, user.UserName!),
            };

            int expireMinutes = int.Parse(jwt["ExpireMinutes"]!);
            DateTime expires = DateTime.UtcNow.AddMinutes(expireMinutes);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
