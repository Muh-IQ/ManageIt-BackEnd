using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_Tier.Repository
{
    public interface IUserAuthorizationRepository
    {
        Task<bool> IsUserAuthorizedForProject(int userId, int ProjectId);

        Task<bool> IsUserAuthorizedForProjectChat(int userId, int groupId);
        Task<bool> IsUserAuthorizedForTaskChat(int userId, int groupId);
    }
}
