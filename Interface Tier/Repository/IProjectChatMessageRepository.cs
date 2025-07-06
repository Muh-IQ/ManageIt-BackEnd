using Interface_Tier.DTOs.Project_Chat_Member_DTOs;
using Interface_Tier.DTOs.Project_Chat_Message_DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_Tier.Repository
{
    public interface IProjectChatMessageRepository
    {
        Task<IEnumerable<ProjectChatMessagePageDTOs>> GetProjectChatMessagesPaged(int ProjectID, int pageNumber, int pageSize);
        Task<bool> AddMessage(int UserID , int ProjectID,string message);
        Task<int> GetCountProjectChatMessage(int ProjectID);
    }
}
