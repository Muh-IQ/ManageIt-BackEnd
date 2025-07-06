using Interface_Tier.DTOs.Project_Member_DTOs;
using Interface_Tier.Repository;
using Interface_Tier.Service;
using Interface_Tier.Utiltiy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Business_Tier
{
    public class ProjectMemberService(IProjectMemberRepository repository , IUserService service) : IProjectMemberService
    {
        public async Task<bool> AddProjectMember(AddProjectMemberServiceDTO memberDTO,int requestUserID)
        {

            // This method adds a user to the ProjectMember table.
            // If the user is given a permission other than "Member" (which is 1),
            // like "Owner" (2) or "TaskLeader" (3), 
            // then the user will also be added to the ProjectChatMember table.
            // If the permission is only "Member", the user is added to ProjectMember only.
            // Note: It's not allowed to add a user as "Owner" from here.


            EnsureHeNeedRightPermission(memberDTO.Permission);

            // get user id by email 
            int UserID = await EnsureEmailExistsAndGetUserID(memberDTO.Email);
            

            // Check if user is already in project
            await EnsureUserIsNotAlreadyInProject(UserID,memberDTO.ProjectID);


            // Check if the requesting user has permission
            await EnsureUserHasPermissionToAddInProject(requestUserID, memberDTO.ProjectID);


            AddProjectMemberRepoDTO repoDTO = new AddProjectMemberRepoDTO 
            {
                Permission = memberDTO.Permission,
                ProjectID = memberDTO.ProjectID,
                UserID = UserID
            };
            return await repository.AddProjectMember(repoDTO);
        }
        public async Task<Permission?> GetPermissionForProjectMember(int UserID, int projectID)
        {
            return await repository.GetPermissionForProjectMember(UserID,projectID);
        }
        public async Task<bool> IsUserAlreadyInProject(int UserID, int projectID)
        {
            return await repository.IsUserAlreadyInProject(UserID, projectID);
        }
        public async Task<IEnumerable<ProjectMemberPageDTO>> GetProjectMembersPaged(int ProjectID, int PageNumber, int PageSize)
        {
            return await repository.GetProjectMembersPaged(ProjectID, PageNumber, PageSize);
        }
        public async Task<int> GetCountProjectMembers(int projectID)
        {
            return await repository.GetCountProjectMembers(projectID);
        }
        public async Task<bool> SetProjectMemberPermission(int UserIDOfRequest, int projectID, int ProjectMemberID, Permission permission)
        {
            EnsureHeNeedRightPermission(permission);

            await EnsureUserHasPermissionToSetPermission(UserIDOfRequest, projectID);


            return await repository.SetProjectMemberPermission(ProjectMemberID, permission);
        }
        public async Task<Permission> GetPermissionForProjectMember(int ProjectMemberID)
        {
            return await repository.GetPermissionForProjectMember(ProjectMemberID);
        }
        public async Task<bool> RemoveProjectMember(int UserIdOfRequest, int projectID, int ProjectMemberID)
        {
            await EnsureUserHasPermissionToRemoveMember(UserIdOfRequest, projectID);

            return await repository.RemoveProjectMember(ProjectMemberID);
        }
        public async Task<IEnumerable<ProjectMemberPageDTO>> GetProjectMembersOutsideTaskMember(int UserIDOfRequest, int ProjectID, int TaskID, int pageNumber, int pageSize)
        {
            await EnsureUserHasPermissionToGetMembers(UserIDOfRequest, ProjectID);
            return await repository.GetProjectMembersOutsideTaskMember(pageNumber, pageSize,ProjectID, TaskID);
        }
        public async Task<int> GetCountProjectMembersOutsideTaskMember(int UserIDOfRequest, int ProjectID, int TaskID)
        {
            await EnsureUserHasPermissionToGetMembers(UserIDOfRequest, ProjectID);
            return await repository.GetCountProjectMembersOutsideTaskMember(ProjectID, TaskID);
        }






        private async Task EnsureUserIsNotAlreadyInProject(int userID, int projectID)
        {
            if (await IsUserAlreadyInProject(userID, projectID))
                throw new ArgumentException("This person already exists in this project");
        }
        private async Task EnsureUserHasPermissionToAddInProject(int userID, int projectID)
        {
            Permission? permission = await GetPermissionForProjectMember(userID, projectID);
            if (permission == null || permission == Permission.Member)
                throw new UnauthorizedAccessException("You do not have permission to add a member to this project.");
        }
        private async Task<int> EnsureEmailExistsAndGetUserID(string Email)
        {
            int UserID = await service.GetUserIDByEmail(Email);
            if (UserID < 1)
                throw new ArgumentException("Email does not exist.");

            return UserID;
        }
        private void EnsureHeNeedRightPermission(Permission permission)
        {
            if (permission == Permission.Owner)
                throw new InvalidOperationException("Cannot assign Owner permission. Only one owner is allowed.");
        }
        private async Task EnsureUserHasPermissionToSetPermission(int UserIDOfRequest, int projectID)
        {
            Permission? per = await GetPermissionForProjectMember(UserIDOfRequest, projectID);
            if (per == null || per != Permission.Owner)
                throw new UnauthorizedAccessException("You do not have permission to Set Permission.");
        }
        private async Task EnsureUserHasPermissionToRemoveMember(int UserIDOfRequest, int projectID)
        {
            Permission? per = await GetPermissionForProjectMember(UserIDOfRequest, projectID);
            if (per == null || per == Permission.Member)
                throw new UnauthorizedAccessException("You do not have permission to Remove Member.");
        }
        private async Task EnsureUserHasPermissionToGetMembers(int UserIDOfRequest, int projectID)
        {
            Permission? per = await GetPermissionForProjectMember(UserIDOfRequest, projectID);
            if (per == null || per == Permission.Member)
                throw new UnauthorizedAccessException("You do not have permission to Get Members.");
        }

       
    }
}
