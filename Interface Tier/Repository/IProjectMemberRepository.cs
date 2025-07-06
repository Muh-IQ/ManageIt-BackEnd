using Interface_Tier.DTOs.Project_DTOs;
using Interface_Tier.DTOs.Project_Member_DTOs;
using Interface_Tier.Utiltiy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_Tier.Repository
{
    public  interface IProjectMemberRepository
    {
        Task<bool> AddProjectMember(AddProjectMemberRepoDTO memberDTO);
        Task<bool> IsUserAlreadyInProject(int UserID , int projectID);
        Task<Permission?> GetPermissionForProjectMember(int UserID , int projectID);
        Task<Permission> GetPermissionForProjectMember(int ProjectMemberID);
        Task<IEnumerable<ProjectMemberPageDTO>> GetProjectMembersPaged(int ProjectID, int PageNumber, int PageSize);
        Task<int> GetCountProjectMembers(int projectID);
        Task<bool> SetProjectMemberPermission( int ProjectMemberID, Permission permission);
        Task<bool> RemoveProjectMember( int ProjectMemberID);
        Task<IEnumerable<ProjectMemberPageDTO>> GetProjectMembersOutsideTaskMember(int pageNumber, int pageSize, int ProjectID, int TaskID);
        Task<int> GetCountProjectMembersOutsideTaskMember(int ProjectID, int TaskID);
    }
}
