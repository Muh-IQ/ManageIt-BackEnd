using Interface_Tier.DTOs.Project_Chat_Message_DTOs;
using Interface_Tier.Repository;
using Interface_Tier.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Tier
{
    public class ProjectChatMessageService(IProjectChatMessageRepository repository) : IProjectChatMessageService
    {
        public async Task<bool> AddMessage(int UserID, int ProjectID, string message)
        {
            return await repository.AddMessage(UserID, ProjectID,    message);
        }

        public async Task<int> GetCountProjectChatMessage(int ProjectID)
        {
            return await repository.GetCountProjectChatMessage(ProjectID);
        }

        public async Task<IEnumerable<ProjectChatMessagePageDTOs>> GetProjectChatMessagesPaged(int ProjectID, int pageNumber, int pageSize)
        {
            return await repository.GetProjectChatMessagesPaged(ProjectID, pageNumber, pageSize);
        }
    }
}
