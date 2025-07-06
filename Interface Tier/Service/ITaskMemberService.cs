using Interface_Tier.DTOs;
using Interface_Tier.DTOs.Project_Member_DTOs;
using Interface_Tier.DTOs.Task_Member_DTOs;
using Interface_Tier.Utiltiy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_Tier.Service
{
    public interface ITaskMemberService
    {
        Task<bool> AddTaskMember(int UserIDOfRequest,AddTaskMemberDTO dTO);
        Task<bool> IsUserAlreadyInTaskMember(int TaskID, int ProjectMemberID);
        Task<Permission> GetPermissionOfTaskMember(int UserID, int TaskMemberID);
        Task<Permission?> GetPermissionOfTaskMemberByTaskID(int UserIDOfRequest, int TaskID);
        Task<bool> DeleteTaskMember(int UserIDOfRequest ,int TaskMemberID);
        Task<int> GetCountTaskMembers(int UserIDOfRequest, int TaskID);
        Task<bool> ChangeTaskMemberPermission(int UserIDOfRequest, ChangeTaskMemberPermissionDTO dTO);
        Task<IEnumerable<TaskMemberPageDTO>> GetTaskMembersPaged(int UserIDOfRequest, int PageNumber, int PageSize, int TaskID);
    }
}
