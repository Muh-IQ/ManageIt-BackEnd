using Interface_Tier.DTOs;
using Interface_Tier.DTOs.Project_Member_DTOs;
using Interface_Tier.DTOs.Task_List_DTOs;
using Interface_Tier.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Presentation_Tier.RequestDTOs;

namespace Presentation_Tier.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskListController(ITaskListService service) : ControllerBase
    {
        [HttpPost("AddTaskList")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> AddTaskList(AddTaskListRequestDTO dTO)
        {
            if (string.IsNullOrEmpty(dTO.Name) || dTO.ProjectID < 1)
            {
                return BadRequest("data is requierd.");
            }
            try
            {
                

                string userId = User.FindFirst("userID")?.Value;
                //string userId = "5";
                AddTaskListDTO listDTO = new AddTaskListDTO()
                {
                    Name = dTO.Name,
                    ProjectID = dTO.ProjectID,
                    CreatedBy = int.Parse(userId)

                };

                bool Result = await service.AddTaskList(listDTO); ;

                if (!Result)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred in AddTaskList");
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

        [HttpPut("UpdateTaskList")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> UpdateTaskList(UpdateTaskListRequestDTO dTO)
        {
            if (string.IsNullOrEmpty(dTO.Name) || dTO.ProjectID < 1 || dTO.TaskListID < 1)
            {
                return BadRequest("data is requierd.");
            }
            try
            {


                string userId = User.FindFirst("userID")?.Value;
                //string userId = "5";
                UpdateTaskListDTO listDTO = new UpdateTaskListDTO()
                {
                    Name = dTO.Name,
                    ProjectID = dTO.ProjectID,
                    TaskListID = dTO.TaskListID,

                };

                bool Result = await service.UpdateTaskList(int.Parse(userId), listDTO); ;

                if (!Result)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred in UpdateTaskList");
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

        [HttpDelete("DeleteTasksList")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> DeleteTasksList(int TaskListID)
        {
            if (TaskListID < 1)
            {
                return BadRequest("Task List ID is requierd.");
            }
            try
            {


                string userId = User.FindFirst("userID")?.Value;
                //string userId = "5";


                bool Result = await service.DeleteTasksList(int.Parse(userId), TaskListID); ;

                if (!Result)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred in DeleteTasksList");
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

        [HttpGet("GetTaskListsWithTasks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<TaskListWithTasksDTO>>> GetTaskListsWithTasks(int ProjectID)
        {
            if (ProjectID < 1)
            {
                return BadRequest("Project ID is requierd.");
            }
            try
            {


                string userId = User.FindFirst("userID")?.Value;
                //string userId = "5";


                IEnumerable<TaskListWithTasksDTO> Result = await service.GetTaskListsWithTasks(int.Parse(userId), ProjectID); ;

                if (!Result.Any())
                {
                    return NoContent();
                }

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
