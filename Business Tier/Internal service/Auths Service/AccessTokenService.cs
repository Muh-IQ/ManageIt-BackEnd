using Interface_Tier.Service.Internal_service.Auths_Service;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Interface_Tier.DTOs;

namespace Business_Tier.Internal_service.Auths_Service
{
    public class AccessTokenService(IRefreshTokenService refreshTokenService) : IAccessTokenService
    {
        public string GenerateAccessToken(int userId)
        {
            AuthsCredentials credentials = AuthsCredentials.Instance;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(credentials.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim("userID", userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var token = new JwtSecurityToken(
                issuer: credentials.Issuer,
                audience: credentials.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(credentials.AccessTokenMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> GetAccessToken(string RefreshToken)
        {
            RefreshTokenDTO tokenDTO = await refreshTokenService.GetRefreshToken(RefreshToken);
            if (tokenDTO.IsRevoked || tokenDTO.ExpiryDate < DateTime.UtcNow)
                return null;

            return GenerateAccessToken(tokenDTO.UserId);
        }
    }
}
