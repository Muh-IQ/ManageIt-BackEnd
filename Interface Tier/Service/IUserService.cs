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
        Task<bool> UpdateUser(UpdateUserServiceDTO dTO);
        Task<bool> IsEmailVerifiedExists(string email);
        Task<(string AccessToken, string RefreshToken)> Login(string email, string password);
        Task<string> GetAccessToken(string RefreshToken);
        Task<int> GetUserIDByEmail(string email);
        Task<SimpleUserDataDTO> GetSimpleUserData(int UserID);
        Task<UserDTO> GetUser(int UserID);
        Task<bool> IsPasswordMatched(int UserID, string Password);
        Task<bool> UpdatePassword(int UserID, string OldPassword, string Password);
    }
}
