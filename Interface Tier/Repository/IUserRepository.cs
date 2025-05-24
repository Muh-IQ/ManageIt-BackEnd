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
        Task<bool> IsEmailVerifiedExists(string email);
        Task<int> Login(string email, string password);

    }
}
