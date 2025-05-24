using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_Tier.Service.Internal_service.Auths_Service
{
    public interface IAccessTokenService
    {
        string GenerateAccessToken(int userId);
        Task<string> GetAccessToken(string RefreshToken);
    }
}
