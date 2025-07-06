using Interface_Tier.DTOs.Project_DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_Tier.Service
{
    public interface IProjectService
    {
        Task<ProjectDetailsDTO> GetProjectDetails(int UserID,int projectID);
        Task<bool> UpdateProject(int UserID, UpdateProjectDTO projectDTO);

        Task<bool> AddProject(AddProjectDTO projectDTO);
        Task<IEnumerable<UserProjectsPagedDTO>> GetUserProjectsPaged(int UserID, int pageNumber, int pageSize);
        Task<int> GetCountUserProjects(int UserID);

        Task<IEnumerable<ProjectStatuseDTO>> GetProjectStatuses();
    }
}
