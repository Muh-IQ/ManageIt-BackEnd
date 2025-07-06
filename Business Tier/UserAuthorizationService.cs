using Interface_Tier.Repository;
using Interface_Tier.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Tier
{
    public class UserAuthorizationService(IUserAuthorizationRepository repository) : IUserAuthorizationService
    {
        public async Task<bool> IsUserAuthorizedForProject(int userId, int ProjectId)
        {
            return await repository.IsUserAuthorizedForProject(userId, ProjectId);
        }

        public async Task<bool> IsUserAuthorizedForProjectChat(int userId, int ProjectId)
        {
            return await repository.IsUserAuthorizedForProjectChat(userId, ProjectId);
        }

        public async Task<bool> IsUserAuthorizedForTaskChat(int userId, int TaskId)
        {
            return await repository.IsUserAuthorizedForTaskChat(userId, TaskId);
        }
       
    }
}
