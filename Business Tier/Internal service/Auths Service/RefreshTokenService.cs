using Interface_Tier.DTOs;
using Interface_Tier.Repository;
using Interface_Tier.Service.Internal_service.Auths_Service;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Business_Tier.Internal_service.Auths_Service
{
    public class RefreshTokenService(IRefreshTokenRepository repository) : IRefreshTokenService
    {
        

        public async Task<(bool IsAdded, string Token)> AddNewRefreshToken(int userId)
        {
            string token = GenerateRefreshToken();
            
            bool IsAdded = await repository.AddNewRefreshToken(userId, token, GetRefreshTokenExpiration());
            return (IsAdded, token);
        }

        public async Task<RefreshTokenDTO> GetRefreshToken(string refreshToken)
        {
            return await repository.GetRefreshToken(refreshToken);
        }

        public DateTime GetRefreshTokenExpiration()
        {
            return DateTime.UtcNow.AddDays(AuthsCredentials.Instance.RefreshTokenDays);
        }

        public string GenerateRefreshToken()
        {
            int length = 32;
            var token = new StringBuilder(length);
            var buffer = new byte[length];
            char[] ValidCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();
            RandomNumberGenerator.Fill(buffer); 

            for (int i = 0; i < length; i++)
            {
                token.Append(ValidCharacters[buffer[i] % ValidCharacters.Length]);
            }

            return token.ToString();
        }


    }
}
