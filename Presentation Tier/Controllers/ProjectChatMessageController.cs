using Interface_Tier.DTOs.Project_Chat_Member_DTOs;
using Interface_Tier.DTOs.Project_Chat_Message_DTOs;
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
    public class ProjectChatMessageController(IProjectChatMessageService service) : ControllerBase
    {
        [HttpGet("GetProjectChatMessagesPaged")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ProjectChatMessagePageDTOs>>> GetProjectChatMessagesPaged(int ProjectID, int PageNumber = 1, int PageSize = 15)
        {
            if (ProjectID < 1 || PageNumber < 1 || PageSize < 1)
            {
                return BadRequest("data is requierd.");
            }
            try
            {

                IEnumerable<ProjectChatMessagePageDTOs> members = await service.GetProjectChatMessagesPaged(ProjectID, PageNumber, PageSize);

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


        [HttpGet("GetCountProjectChatMessage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> GetCountProjectChatMessage(int ProjectID)
        {
            if (ProjectID < 1 )
            {
                return BadRequest("Project ID is requierd.");
            }
            try
            {
                int Result = await service.GetCountProjectChatMessage(ProjectID);

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
