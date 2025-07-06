using Interface_Tier.DTOs;
using Interface_Tier.DTOs.Project_DTOs;
using Interface_Tier.DTOs.Task_Member_DTOs;
using Interface_Tier.Service;
using Interface_Tier.Utiltiy;
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
    public class TaskMemberController(ITaskMemberService service) : ControllerBase
    {
        [HttpPost("AddTaskMember")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> AddTaskMember(AddTaskMemberRequestDTO dTO)
        {
            if (dTO.TaskID < 1 || dTO.ProjectMemberID < 1 || (byte)dTO.permission > 3 || (byte)dTO.permission < 1)
            {
                return BadRequest("data is requierd. or wrong");
            }
            try
            {

                string userId = User.FindFirst("userID")?.Value;
                //string userId = "5";

                bool Result = await service.AddTaskMember(int.Parse(userId), dTO); ;

                if (!Result)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred in AddTaskMember");
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

        [HttpPut("ChangeTaskMemberPermission")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> ChangeTaskMemberPermission(ChangeTaskMemberPermissionRequestDTO dTO)
        {
            if (dTO.TaskMemberID < 1 || (byte)dTO.permission > 3 || (byte)dTO.permission < 1)
            {
                return BadRequest("data is requierd. or wrong");
            }
            try
            {

                string userId = User.FindFirst("userID")?.Value;
                //string userId = "5";

                bool Result = await service.ChangeTaskMemberPermission(int.Parse(userId), dTO); ;

                if (!Result)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred in Change TaskMember Permission");
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

        [HttpDelete("DeleteTaskMember")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> DeleteTaskMember(int TaskMemberID)
        {
            if (TaskMemberID < 1 )
            {
                return BadRequest("TaskMemberID is requierd.");
            }
            try
            {

                string userId = User.FindFirst("userID")?.Value;
                //string userId = "5";

                bool Result = await service.DeleteTaskMember(int.Parse(userId), TaskMemberID); ;

                if (!Result)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred in Delete TaskMember ");
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

        [HttpGet("GetTaskMembersPaged")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<TaskMemberPageDTO>>> GetTaskMembersPaged(int TaskID,int PageNumber = 1, int PageSize = 15)
        {
            if (PageNumber < 1 || TaskID < 1 || PageSize < 1)
            {
                return BadRequest("data is requierd.");
            }
            try
            {
                string userId = User.FindFirst("userID")?.Value;
                //string userId = "10";
                IEnumerable <TaskMemberPageDTO> Members = await service.GetTaskMembersPaged(int.Parse(userId), PageNumber, PageSize, TaskID);

                if (Members == null || !Members.Any())
                {
                    return NotFound("not found data");
                }

                return Ok(Members);
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

        [HttpGet("GetCountTaskMembers")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> GetCountTaskMembers(int TaskID)
        {
            if (TaskID < 1 )
            {
                return BadRequest("TaskID is requierd.");
            }
            try
            {
                string userId = User.FindFirst("userID")?.Value;
                //string userId = "5";
                int count = await service.GetCountTaskMembers(int.Parse(userId), TaskID);

                return Ok(count);
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
