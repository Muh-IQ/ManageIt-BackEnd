using Interface_Tier.DTOs.Project_DTOs;
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
    public class ProjectService(IProjectRepository repository ,IUserAuthorizationService authorizationService
        , IProjectMemberService service) : IProjectService
    {
        public async Task<bool> AddProject(AddProjectDTO projectDTO)
        {
            return await repository.AddProject(projectDTO);
        }

        public async Task<int> GetCountUserProjects(int UserID)
        {
            return await repository.GetCountUserProjects(UserID);
        }

        public async Task<ProjectDetailsDTO> GetProjectDetails(int UserID,int projectID)
        {
            // is this uesr authorized to get project data
            bool IsAuthorized = await authorizationService.IsUserAuthorizedForProject(UserID, projectID);
            if (!IsAuthorized)
                return null;

            return await repository.GetProjectDetails(projectID);
        }

        public async Task<IEnumerable<ProjectStatuseDTO>> GetProjectStatuses()
        {
             return await repository.GetProjectStatuses();
        }

        public async Task<IEnumerable<UserProjectsPagedDTO>> GetUserProjectsPaged(int UserID, int pageNumber, int pageSize)
        {
            return await repository.GetUserProjectsPaged(UserID, pageNumber, pageSize);
        }

        public async Task<bool> UpdateProject(int UserID, UpdateProjectDTO projectDTO)
        {
            await EnsureUserHasPermissionToUpdateProject(UserID, projectDTO.ProjectsID);
            return await repository.UpdateProject(projectDTO);
        }
        private async Task EnsureUserHasPermissionToUpdateProject(int userID, int projectID)
        {
            Permission? permission = await service.GetPermissionForProjectMember(userID, projectID);
            if (permission == null || permission != Permission.Owner)
                throw new UnauthorizedAccessException("You do not have permission to Update project.");
        }
    }
}
