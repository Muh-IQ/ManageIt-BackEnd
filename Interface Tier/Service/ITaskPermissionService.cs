using Interface_Tier.Utiltiy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_Tier.Service
{
    public interface ITaskPermissionService
    {
        Task<Permission?> GetPermissionOfTask(int taskId, int userId);
        Task<Permission?> GetPermissionOfTaskMemberByTaskID(int userId, int taskId );
    }
}
