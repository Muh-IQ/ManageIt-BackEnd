using Interface_Tier.DTOs.Project_Chat_Member_DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_Tier.Repository
{
    public interface IProjectChatMemberRepository
    {
        Task<IEnumerable<ProjectChatMemberPageDTO>> GetProjectChatMembersPaged(int ProjectID, int pageNumber, int pageSize);
        Task<int> GetCountProjectChatMembers(int ProjectID);
    }
}
