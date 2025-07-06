using Interface_Tier.DTOs;
using Interface_Tier.DTOs.Task_List_DTOs;
using Interface_Tier.Repository;
using Interface_Tier.Service;
using Interface_Tier.Utiltiy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Tier
{
    public class TaskListService (ITaskListRepository repository , IProjectMemberService service) : ITaskListService
    {
        public async Task<bool> AddTaskList(AddTaskListDTO dTO)
        {
            await EnsureUserHasPermissionToAddTaskList(dTO.CreatedBy,dTO.ProjectID);
            return await repository.AddTaskList(dTO);
        }

        public async Task<bool> DeleteTasksList(int UserIDOfRequest, int TaskListID)
        {
            await EnsureUserHasPermissionToDeleteTaskList(UserIDOfRequest, TaskListID);
            return await repository.DeleteTasksList(TaskListID);
        }

        public async Task<Permission> GetPermissionOfTaskList(int UserID, int TaskListID)
        {
            return await repository.GetPermissionOfTaskList(UserID, TaskListID);
        }
        public async Task<bool> UpdateTaskList(int UserIDOfRequest, UpdateTaskListDTO dTO)
        {
            await EnsureUserHasPermissionToUpdateTaskList(UserIDOfRequest, dTO.ProjectID);
            return await repository.UpdateTaskList(dTO);
        }
        public async Task<IEnumerable<TaskListWithTasksDTO>> GetTaskListsWithTasks(int UserIDOfRequest, int ProjectID)
        {
            await EnsureUserHasPermissionToGetTaskListsWithTasks(UserIDOfRequest, ProjectID);
            return await repository.GetTaskListsWithTasks(UserIDOfRequest, ProjectID);
        }



        private async Task EnsureUserHasPermissionToAddTaskList(int UserIDOfRequest, int projectID)
        {
            Permission? per = await service.GetPermissionForProjectMember(UserIDOfRequest, projectID);
            if (per == null || per == Permission.Member)
                throw new UnauthorizedAccessException("You do not have permission to Add Task List.");
        }

        private async Task EnsureUserHasPermissionToUpdateTaskList(int UserIDOfRequest, int projectID)
        {
            Permission? per = await service.GetPermissionForProjectMember(UserIDOfRequest, projectID);
            if (per == null || per == Permission.Member)
                throw new UnauthorizedAccessException("You do not have permission to update Task List.");
        }
        private async Task EnsureUserHasPermissionToGetTaskListsWithTasks(int UserIDOfRequest, int projectID)
        {
            Permission? per = await service.GetPermissionForProjectMember(UserIDOfRequest, projectID);
            if (per == null)
                throw new UnauthorizedAccessException("You do not have permission to Get Task Lists With Tasks.");
        }
        private async Task EnsureUserHasPermissionToDeleteTaskList(int UserIDOfRequest, int TaskListID)
        {
            Permission per = await GetPermissionOfTaskList(UserIDOfRequest, TaskListID);
            if (per == Permission.Member)
                throw new UnauthorizedAccessException("You do not have permission to Delete Task List.");
        }


       
    }
}
