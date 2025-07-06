using Interface_Tier.DTOs.Project_DTOs;
using Interface_Tier.DTOs.Project_Member_DTOs;
using Interface_Tier.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Presentation_Tier.RequestDTOs;
using System.Collections.Generic;

namespace Presentation_Tier.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectController(IProjectService service) : ControllerBase
    {
        [HttpPost("AddProject")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddProject(AddProjectRequestDTO dTO)
        {
            if (string.IsNullOrEmpty(dTO.Name) || string.IsNullOrEmpty(dTO.Description) || string.IsNullOrEmpty(dTO.Requirements))
            {
                return BadRequest("data is requierd.");
            }
            try
            {
                // get userid form token
                string userId = User.FindFirst("userID")?.Value;

                AddProjectDTO projectDTO = new AddProjectDTO
                {
                    UserID = int.Parse(userId),
                    Requirements = dTO.Requirements,
                    Name = dTO.Name,
                    Description = dTO.Description,
                };

                bool Result = await service.AddProject(projectDTO); ;

                if (!Result)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }



                return NoContent();
            }
            catch (SqlException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred: {ex.Message}");
            }
        }

        [HttpPut("UpdateProject")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateProject(UpdateProjectRequestDTO dTO)
        {
            if (string.IsNullOrEmpty(dTO.Name) || string.IsNullOrEmpty(dTO.Description) 
                || string.IsNullOrEmpty(dTO.Requirements) || dTO.ProjectStatuseID < 1 || dTO.ProjectsID < 1)
            {
                return BadRequest("data is requierd.");
            }
            try
            {
                // get userid form token
                string userId = User.FindFirst("userID")?.Value;

              
                bool Result = await service.UpdateProject(int.Parse(userId), dTO); ;

                if (!Result)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }



                return NoContent();
            }
            catch (SqlException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred: {ex.Message}");
            }
        }

        [HttpGet("GetUserProjectsPaged")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<UserProjectsPagedDTO>>> GetUserProjectsPaged(int PageNumber = 1, int PageSize = 15)
        {
            if (PageNumber < 1 || PageSize < 1)
            {
                return BadRequest("data is requierd.");
            }
            try
            {
                string userId = User.FindFirst("userID")?.Value;

                IEnumerable< UserProjectsPagedDTO > projects = await service.GetUserProjectsPaged(int.Parse(userId), PageNumber, PageSize);

                if (projects == null || !projects.Any())
                {
                    return NotFound("not found data");
                }

                return Ok(projects);
            }
            catch (SqlException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred: {ex.Message}");
            }
        }

        [HttpGet("GetCountUserProjects")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> GetCountUserProjects()
        {
        
            try
            {
                string userId = User.FindFirst("userID")?.Value;

                int Result = await service.GetCountUserProjects((int.Parse(userId))) ;

                return Ok(Result);
            }
            catch (SqlException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred: {ex.Message}");
            }
        }

        [HttpGet("GetProjectDetails")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProjectDetailsDTO>> GetProjectDetails(int projectID)
        {
            if ( projectID < 1)
            {
                return BadRequest("projectID is requierd.");
            }

            try
            {
                string userId = User.FindFirst("userID")?.Value;
                //string userId = "5";

                ProjectDetailsDTO dTO = await service.GetProjectDetails((int.Parse(userId)), projectID);
                if (dTO == null)
                    return NotFound();

                return Ok(dTO);
            }
            catch (SqlException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred: {ex.Message}");
            }
        }


        [HttpGet("GetProjectStatuses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ProjectStatuseDTO>>> GetProjectStatuses()
        {
            try
            {

                IEnumerable<ProjectStatuseDTO> projects = await service.GetProjectStatuses();

                if (projects == null || !projects.Any())
                {
                    return NotFound("not found data");
                }

                return Ok(projects);
            }
            catch (SqlException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}
