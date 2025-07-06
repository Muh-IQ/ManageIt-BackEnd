using Interface_Tier.DTOs.Task_Chat_Message_DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_Tier.Repository
{
    public interface ITaskChatMessageRepository
    {
        Task<bool> AddMessage(int userId, int taskId, string message);
        Task<IEnumerable<TaskChatMessagePageDTOs>> GetTaskChatMessagesPaged(int taskId, int PageNumber, int PageSize);
        Task<int> GetCountTaskChatMessagesPaged(int taskId);
    }
}
