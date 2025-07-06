using Interface_Tier.DTOs;
using Interface_Tier.DTOs.Task_List_DTOs;
using Interface_Tier.Utiltiy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_Tier.Service
{
    public interface ITaskListService
    {
        Task<bool> AddTaskList(AddTaskListDTO dTO);
        Task<bool> UpdateTaskList(int UserIDOfRequest,UpdateTaskListDTO dTO);
        Task<Permission> GetPermissionOfTaskList(int UserID, int TaskListID);
        Task<bool> DeleteTasksList(int UserIDOfRequest, int TaskListID);
        Task<IEnumerable<TaskListWithTasksDTO>> GetTaskListsWithTasks(int UserIDOfRequest, int ProjectID);
    }
}
