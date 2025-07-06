using Interface_Tier.DTOs.Task_Chat_Message_DTOs;
using Interface_Tier.Repository;
using Interface_Tier.Service;
using Interface_Tier.Utiltiy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Tier
{
    public class TaskChatMessageService(ITaskChatMessageRepository repository , ITaskPermissionService permissionService) : ITaskChatMessageService
    {
        public async Task<bool> AddMessage(int userId, int taskId, string message)
        {
            return await repository.AddMessage(userId, taskId, message);
        }

        public async Task<int> GetCountTaskChatMessagesPaged(int userIdOfRequest,int taskId)
        {
            await EnsureUserHasPermissionToGetTaskMessages(userIdOfRequest, taskId);
            return await repository.GetCountTaskChatMessagesPaged(taskId);
        }

        public async Task<IEnumerable<TaskChatMessagePageDTOs>> GetTaskChatMessagesPaged(int userIdOfRequest, int taskId, int PageNumber, int PageSize)
        {
            await EnsureUserHasPermissionToGetTaskMessages(userIdOfRequest, taskId);
            return await repository.GetTaskChatMessagesPaged(taskId, PageNumber, PageSize);
        }

        private async Task EnsureUserHasPermissionToGetTaskMessages(int UserIDOfRequest , int TaskID)
        {
            Permission? per = await permissionService.GetPermissionOfTaskMemberByTaskID( UserIDOfRequest, TaskID);
            if (per == null)
                throw new UnauthorizedAccessException("You do not have permission to Get Task Messages.");
        }
    }
}
