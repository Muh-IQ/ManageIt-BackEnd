using Interface_Tier.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_Tier.Service.Internal_service.Auths_Service
{
    public interface IRefreshTokenService
    {
        Task<RefreshTokenDTO> GetRefreshToken(string refreshToken);
        Task<(bool IsAdded, string Token)> AddNewRefreshToken(int userId);
        string GenerateRefreshToken();
        DateTime GetRefreshTokenExpiration();
    }
}
