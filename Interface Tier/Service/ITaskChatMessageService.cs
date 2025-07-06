using Interface_Tier.DTOs.Task_Chat_Message_DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_Tier.Service
{
    public interface ITaskChatMessageService
    {
        Task<bool> AddMessage(int userId, int taskId, string message);
        Task<IEnumerable<TaskChatMessagePageDTOs>> GetTaskChatMessagesPaged(int userIdOfRequest, int taskId, int PageNumber, int PageSize);
        Task<int> GetCountTaskChatMessagesPaged(int userIdOfRequest, int taskId);
    }
}
