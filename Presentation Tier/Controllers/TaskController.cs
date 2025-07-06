using Interface_Tier.DTOs;
using Interface_Tier.DTOs.Task_DTOs;
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
    public class TaskController(ITaskService service) : ControllerBase
    {
        [HttpPost("AddTask")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> AddTask(AddTaskRequestDTO dTO)
        {
            if (string.IsNullOrEmpty(dTO.Name) || string.IsNullOrEmpty(dTO.Description) || dTO.TaskListID < 1)
            {
                return BadRequest("data is requierd.");
            }
            try
            {

                string userId = User.FindFirst("userID")?.Value;
                //string userId = "5";
                AddTaskDTO TaskDTO = new AddTaskDTO()
                {
                    Name = dTO.Name,
                    CreatedBy = int.Parse(userId),
                    Description = dTO.Description,
                    TaskListID = dTO.TaskListID,
                };

                bool Result = await service.AddTask(TaskDTO); ;

                if (!Result)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred in AddTask");
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

        [HttpPut("UpdateTask")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> UpdateTask(UpdateTaskRequestDTO dTO)
        {
            if (string.IsNullOrEmpty(dTO.Name) || string.IsNullOrEmpty(dTO.Description) || dTO.TaskID < 1)
            {
                return BadRequest("data is requierd.");
            }
            try
            {

                string userId = User.FindFirst("userID")?.Value;
                //string userId = "5";
         

                bool Result = await service.UpdateTask(int.Parse(userId), dTO); ;

                if (!Result)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred in UpdateTask");
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

        [HttpGet("GetTaskStatuses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<TaskStatusesDTO>>> GetTaskStatuses()
        {
            try
            {
                IEnumerable<TaskStatusesDTO> Result = await service.GetTaskStatuses(); ;

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

        [HttpDelete("DeleteTask")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> DeleteTask(int TaskID)
        {
            if (TaskID < 1)
            {
                return BadRequest("TaskID is requierd.");
            }
            try
            {
                string userId = User.FindFirst("userID")?.Value;
                //string userId = "5";
                bool isSuccess = await service.DeleteTask(int.Parse(userId),TaskID); ;
                if (!isSuccess)
                {
                    return BadRequest("Task not found or could not be Deleted.");
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

        [HttpPut("ResetStartDateTask")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> ResetStartDateTask(int TaskID)
        {
            if (TaskID < 1)
            {
                return BadRequest("TaskID is requierd.");
            }
            try
            {
                string userId = User.FindFirst("userID")?.Value;
                //string userId = "5";
                bool isSuccess = await service.ResetStartDateTask(int.Parse(userId), TaskID);
                if (!isSuccess)
                {
                    return BadRequest("Task not found or could not be updated.");
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

        [HttpPut("ResetDeliveryDateTask")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> ResetDeliveryDateTask(int TaskID)
        {
            if (TaskID < 1)
            {
                return BadRequest("TaskID is requierd.");
            }
            try
            {
                string userId = User.FindFirst("userID")?.Value;
                //string userId = "5";
                bool isSuccess = await service.ResetDeliveryDateTask(int.Parse(userId), TaskID);
                if (!isSuccess)
                {
                    return BadRequest("Task not found or could not be updated.");
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

        [HttpGet("GetTask")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetTaskDTO>> GetTask(int TaskID)
        {
            if (TaskID < 1)
            {
                return BadRequest("TaskID is requierd.");
            }
            try
            {
                string userId = User.FindFirst("userID")?.Value;
                //string userId = "5";
                GetTaskDTO dto  = await service.GetTask(int.Parse(userId), TaskID);
                if (dto == null)
                {
                    return NotFound("Task not found.");
                }
                return Ok(dto);
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

        [HttpPut("SetDeliveryDateTask")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> SetDeliveryDateTask(int TaskID)
        {
            if (TaskID < 1)
            {
                return BadRequest("TaskID is requierd.");
            }
            try
            {
                string userId = User.FindFirst("userID")?.Value;
                //string userId = "10";
                bool res = await service.SetDeliveryDateTask(int.Parse(userId), TaskID);
             
                return Ok(res);
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

        [HttpPut("SetStartDateTask")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> SetStartDateTask(int TaskID)
        {
            if (TaskID < 1)
            {
                return BadRequest("TaskID is requierd.");
            }
            try
            {
                string userId = User.FindFirst("userID")?.Value;
                //string userId = "10";
                bool res = await service.SetStartDateTask(int.Parse(userId), TaskID);

                return Ok(res);
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
