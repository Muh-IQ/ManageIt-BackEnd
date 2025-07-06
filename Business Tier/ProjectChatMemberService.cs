using Interface_Tier.DTOs.Project_Chat_Member_DTOs;
using Interface_Tier.Repository;
using Interface_Tier.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Tier
{
    public class ProjectChatMemberService(IProjectChatMemberRepository repository) : IProjectChatMemberService
    {
        public async Task<int> GetCountProjectChatMembers(int ProjectID)
        {
            return await repository.GetCountProjectChatMembers(ProjectID);
        }

        public async Task<IEnumerable<ProjectChatMemberPageDTO>> GetProjectChatMembersPaged(int ProjectID, int pageNumber, int pageSize)
        {
            return await repository.GetProjectChatMembersPaged(ProjectID, pageNumber, pageSize);
        }
    }
}
