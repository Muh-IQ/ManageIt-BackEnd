using Interface_Tier.DTOs.User_DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_Tier.Repository
{
    public interface IUserRepository
    {
        Task<int> AddUser(AddUserRepositoryDTO addUserDTO);
        Task<bool> UpdateUser(UpdateUserRepositoryDTO dTO);
        Task<bool> IsEmailVerifiedExists(string email);
        Task<bool> IsPasswordMatched(int UserID, string Password);
        Task<bool> UpdatePassword(int UserID, string Password);
        Task<int> Login(string email, string password);
        Task<int> GetUserIDByEmail(string email);
        Task<UserDTO> GetUser(int UserID);
        Task<SimpleUserDataDTO> GetSimpleUserData(int UserID);

    }
}
