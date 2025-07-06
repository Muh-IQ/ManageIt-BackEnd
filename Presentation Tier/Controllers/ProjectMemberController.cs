using Interface_Tier.DTOs.Project_Member_DTOs;
using Interface_Tier.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Presentation_Tier.RequestDTOs;

namespace Presentation_Tier.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectMemberController(IProjectMemberService service) : ControllerBase
    {
        [HttpPost("AddProjectMember")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> AddProjectMember(AddProjectMemberServiceDTO dTO)
        {
            if (string.IsNullOrEmpty(dTO.Email) || (byte)dTO.Permission < 1 || (byte)dTO.Permission > 3 || dTO.ProjectID < 1 )
            {
                return BadRequest("data is requierd.");
            }
            try
            {
                string userId = User.FindFirst("userID")?.Value;
               

                bool Result = await service.AddProjectMember(dTO, int.Parse(userId)); ;

                if (!Result)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred in add Add Member");
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

        [HttpPost("SetProjectMemberPermission")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> SetProjectMemberPermission(SetProjectMemberPermissionRequestDTO dTO)
        {
            if ((byte)dTO.permission < 1 || (byte)dTO.permission > 3 || dTO.ProjectMemberID < 1)
            {
                return BadRequest("data is requierd.");
            }
            try
            {
                string userId = User.FindFirst("userID")?.Value;
                //string userId = "5";

                bool Result = await service.SetProjectMemberPermission(int.Parse(userId), dTO.ProjectID, dTO.ProjectMemberID, dTO.permission); 

                if (!Result)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred in update permission");
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

        [HttpGet("GetProjectMembersPaged")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ProjectMemberPageDTO>>> GetProjectMembersPaged(int ProjectID, int PageNumber = 1, int PageSize = 15)
        {
            if (ProjectID < 1 || PageNumber < 1 || PageSize < 1)
            {
                return BadRequest("data is requierd.");
            }
            try
            {

                IEnumerable<ProjectMemberPageDTO> members = await service.GetProjectMembersPaged(ProjectID, PageNumber, PageSize); 

                if (members == null || !members.Any())
                {
                    return NotFound("not found data");
                }

                return Ok(members);
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

        [HttpGet("GetCountProjectMembers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> GetCountProjectMembers(int ProjectID)
        {
            if (ProjectID < 1)
            {
                return BadRequest("Project ID is requierd.");
            }
            try
            {

                int Result = await service.GetCountProjectMembers(ProjectID); ;
            

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

        [HttpDelete("RemoveProjectMember")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> RemoveProjectMember(RemoveProjectMemberRequestDTO dTO)
        {
            if (dTO.ProjectID < 1  || dTO.ProjectMemberID < 1)
            {
                return BadRequest("data is requierd.");
            }
            try
            {
                string userId = User.FindFirst("userID")?.Value;
                //string userId = "10";

                bool Result = await service.RemoveProjectMember(int.Parse(userId), dTO.ProjectID, dTO.ProjectMemberID);

                if (!Result)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred in add Add Member");
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

        [HttpGet("GetProjectMembersOutsideTaskMember")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ProjectMemberPageDTO>>> GetProjectMembersOutsideTaskMember(int ProjectID, int TaskID, int PageNumber = 1, int PageSize = 15)
        {
            if (ProjectID < 1 || TaskID < 1 || PageNumber < 1 || PageSize < 1)
            {
                return BadRequest("data is requierd.");
            }
            try
            {
                string userId = User.FindFirst("userID")?.Value;
                //string userId = "10";
                IEnumerable<ProjectMemberPageDTO> members = await service.GetProjectMembersOutsideTaskMember(int.Parse(userId), ProjectID, TaskID, PageNumber, PageSize);

                if (members == null || !members.Any())
                {
                    return NotFound("not found data");
                }

                return Ok(members);
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
        
        [HttpGet("GetCountProjectMembersOutsideTaskMember")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> GetCountProjectMembersOutsideTaskMember(int ProjectID, int TaskID)
        {
            if (ProjectID < 1)
            {
                return BadRequest("Project ID is requierd.");
            }
            try
            {
                string userId = User.FindFirst("userID")?.Value;
                //string userId = "10";
                int Result = await service.GetCountProjectMembersOutsideTaskMember(int.Parse(userId), ProjectID,TaskID); ;

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
    }
}
