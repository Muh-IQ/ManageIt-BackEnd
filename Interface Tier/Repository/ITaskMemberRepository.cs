using Interface_Tier.DTOs;
using Interface_Tier.DTOs.Project_Member_DTOs;
using Interface_Tier.DTOs.Task_Member_DTOs;
using Interface_Tier.Utiltiy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_Tier.Repository
{
    public interface ITaskMemberRepository
    {
        Task<bool> AddTaskMember(AddTaskMemberDTO dTO);
        Task<bool> IsUserAlreadyInTaskMember(int TaskID, int ProjectMemberID);
        Task<bool> ChangeTaskMemberPermission(ChangeTaskMemberPermissionDTO dTO);
        Task<bool> DeleteTaskMember(int TaskMemberID);
        Task<Permission> GetPermissionOfTaskMember(int UserID, int TaskMemberID);
        Task<Permission?> GetPermissionOfTaskMemberByTaskID(int UserID, int TaskID);
        Task<IEnumerable<TaskMemberPageDTO>> GetTaskMembersPaged(int PageNumber, int PageSize,int TaskID);
        Task<int> GetCountTaskMembers(int TaskID);
    }
}
