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
    public class TaskPermissionService( ITaskRepository taskRepository, ITaskMemberRepository taskMemberRepository ) : ITaskPermissionService
    {
        public async Task<Permission?> GetPermissionOfTask(int taskId, int userId)
        {
            return await taskRepository.GetPermissionOfTask(taskId, userId);
        }


        public async Task<Permission?> GetPermissionOfTaskMemberByTaskID(int userId, int taskId)
        {
            return await taskMemberRepository.GetPermissionOfTaskMemberByTaskID(userId, taskId);
        }
    }

}
