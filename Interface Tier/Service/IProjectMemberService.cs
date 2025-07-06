using Interface_Tier.DTOs.Project_Member_DTOs;
using Interface_Tier.Utiltiy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_Tier.Service
{
    public interface IProjectMemberService
    {
        Task<bool> AddProjectMember(AddProjectMemberServiceDTO memberDTO, int requestUserID);
        Task<bool> IsUserAlreadyInProject(int UserID, int projectID);
        Task<Permission?> GetPermissionForProjectMember(int UserID, int projectID);
        Task<Permission> GetPermissionForProjectMember(int ProjectMemberID);

        Task<IEnumerable<ProjectMemberPageDTO>> GetProjectMembersPaged(int ProjectID, int PageNumber, int PageSize);
        Task<int> GetCountProjectMembers(int projectID);
        Task<bool> SetProjectMemberPermission(int UserIDOfRequest, int projectID, int ProjectMemberID, Permission permission);
        Task<bool> RemoveProjectMember(int UserIdOfRequest, int projectID, int ProjectMemberID);
        Task<IEnumerable<ProjectMemberPageDTO>> GetProjectMembersOutsideTaskMember(int UserIDOfRequest, int ProjectID, int TaskID, int pageNumber, int pageSize);
        Task<int> GetCountProjectMembersOutsideTaskMember(int UserIDOfRequest, int ProjectID, int TaskID);

    }
}
