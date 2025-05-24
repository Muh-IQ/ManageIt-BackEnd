using Interface_Tier.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_Tier.Repository
{
    public interface IRefreshTokenRepository
    {
        Task<bool> AddNewRefreshToken(int userId, string refreshToken, DateTime? ExpiryDate);
        Task<RefreshTokenDTO> GetRefreshToken(string refreshToken);
    }
}
