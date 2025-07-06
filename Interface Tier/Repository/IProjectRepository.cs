using Interface_Tier.DTOs.Project_DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_Tier.Repository
{
    public interface IProjectRepository
    {
        Task<bool> AddProject(AddProjectDTO projectDTO);
        Task<bool> UpdateProject(UpdateProjectDTO projectDTO);
        Task<ProjectDetailsDTO> GetProjectDetails(int projectID);
        Task<int> GetCountUserProjects(int UserID);
        Task<IEnumerable<UserProjectsPagedDTO>> GetUserProjectsPaged(int UserID, int pageNumber, int pageSize);
        Task<IEnumerable<ProjectStatuseDTO>> GetProjectStatuses();
    }
}
