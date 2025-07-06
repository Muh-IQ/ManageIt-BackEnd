using Interface_Tier.DTOs.Project_Chat_Member_DTOs;
using Interface_Tier.DTOs.Task_Chat_Message_DTOs;
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
    public class TaskChatMessageController(ITaskChatMessageService service) : ControllerBase
    {
        [HttpGet("GetTaskChatMessagesPaged")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<TaskChatMessagePageDTOs>>> GetTaskChatMessagesPaged(int TaskID, int PageNumber = 1, int PageSize = 15)
        {
            if (TaskID < 1 || PageNumber < 1 || PageSize < 1)
            {
                return BadRequest("data is requierd.");
            }
            try
            {
                string userId = User.FindFirst("userID")?.Value;
                //string userId = "5";
                IEnumerable<TaskChatMessagePageDTOs> messages = await service.GetTaskChatMessagesPaged(int.Parse(userId),TaskID, PageNumber, PageSize);

                if (messages == null || !messages.Any())
                {
                    return NoContent();
                }

                return Ok(messages);
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


        [HttpGet("GetCountTaskChatMessagesPaged")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<TaskChatMessagePageDTOs>>> GetCountTaskChatMessagesPaged(int TaskID)
        {
            if (TaskID < 1)
            {
                return BadRequest("TaskID is requierd.");
            }
            try
            {
                string userId = User.FindFirst("userID")?.Value;
                //string userId = "5";
                int counter = await service.GetCountTaskChatMessagesPaged(int.Parse(userId), TaskID);

             
                return Ok(counter);
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
