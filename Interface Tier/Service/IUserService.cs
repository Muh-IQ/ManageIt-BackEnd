using Interface_Tier.DTOs.User_DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_Tier.Service
{
    public interface IUserService
    {
        Task<int> AddUser(AddUserServiceDTO serviceDTO);
        Task<bool> IsEmailVerifiedExists(string email);
        Task<(string AccessToken, string RefreshToken)> Login(string email, string password);
        Task<string> GetAccessToken(string RefreshToken);
    }
}
