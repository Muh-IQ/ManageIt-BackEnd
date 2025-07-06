using Interface_Tier.DTOs.Project_Chat_Member_DTOs;
using Interface_Tier.DTOs.Project_Member_DTOs;
using Interface_Tier.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Presentation_Tier.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectChatMemberController(IProjectChatMemberService service) : ControllerBase
    {
        [HttpGet("GetProjectChatMembersPaged")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ProjectChatMemberPageDTO>>> GetProjectChatMembersPaged(int ProjectID, int PageNumber = 1, int PageSize = 15)
        {
            if (ProjectID < 1 || PageNumber < 1 || PageSize < 1)
            {
                return BadRequest("data is requierd.");
            }
            try
            {

                IEnumerable<ProjectChatMemberPageDTO> members = await service.GetProjectChatMembersPaged(ProjectID, PageNumber, PageSize);

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

        [HttpGet("GetCountProjectChatMembers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> GetCountProjectChatMembers(int ProjectID)
        {
            if (ProjectID < 1)
            {
                return BadRequest("Project ID is requierd.");
            }
            try
            {
                int Result = await service.GetCountProjectChatMembers(ProjectID);

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
