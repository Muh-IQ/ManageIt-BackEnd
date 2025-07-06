using Interface_Tier.DTOs;
using Interface_Tier.DTOs.Task_DTO;
using Interface_Tier.DTOs.Task_DTOs;
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
    public class TaskService(ITaskRepository repository, ITaskListService service, ITaskPermissionService permissionService) : ITaskService
    {
        public async Task<bool> AddTask(AddTaskDTO dTO)
        {
            await EnsureUserHasPermissionToAddTask(dTO.CreatedBy, dTO.TaskListID);
            dTO.CreationDate = DateTime.Now;
            return await repository.AddTask(dTO);
        }
        public async Task<bool> DeleteTask(int UserIDOfRequest, int TaskID)
        {
            await EnsureUserHasPermissionToDeleteTask(TaskID, UserIDOfRequest);
            return await repository.DeleteTask(TaskID);
        }
        public async Task<Permission?> GetPermissionOfTask(int TaskID, int UserID)
        {
            return await repository.GetPermissionOfTask(TaskID, UserID);
        }
        public async Task<IEnumerable<TaskStatusesDTO>> GetTaskStatuses()
        {
            return await repository.GetTaskStatuses();
        }
        public async Task<bool> UpdateTask(int UserIDOfRequest, UpdateTaskDTO dTO)
        {
            await EnsureUserHasPermissionToUpdateTask(dTO.TaskID,UserIDOfRequest);
            return await repository.UpdateTask(dTO);
        }
        public async Task<bool> ResetStartDateTask(int UserIDOfRequest, int TaskID)
        {
            await EnsureUserHasPermissionToResetStartDateTask(TaskID, UserIDOfRequest);

            return await repository.ResetStartDateTask(TaskID);
        }
        public async Task<bool> ResetDeliveryDateTask(int UserIDOfRequest, int TaskID)
        {
            await EnsureUserHasPermissionToResetDeliveryDateTask(TaskID, UserIDOfRequest);

            return await repository.ResetDeliveryDateTask(TaskID);
        }
        public async Task<GetTaskDTO> GetTask(int UserIDOfRequest, int TaskID)
        {
            await EnsureUserHasPermissionToGetTask(TaskID,UserIDOfRequest);
            return await repository.GetTask(UserIDOfRequest,TaskID);
        }
        public async Task<bool> SetDeliveryDateTask(int UserIDOfRequest, int TaskID)
        {
            await EnsureUserHasPermissionToSetDeliveryDate(UserIDOfRequest, TaskID);
            return await repository.SetDeliveryDateTask(TaskID);
        }
        public async Task<bool> SetStartDateTask(int UserIDOfRequest, int TaskID)
        {
            await EnsureUserHasPermissionToSetStartDate(UserIDOfRequest, TaskID);
            return await repository.SetStartDateTask(TaskID);
        }




        private async Task EnsureUserHasPermissionToAddTask(int UserIDOfRequest, int TaskListID)
        {
            Permission per = await service.GetPermissionOfTaskList(UserIDOfRequest, TaskListID);
            if (per == Permission.Member)
                throw new UnauthorizedAccessException("You do not have permission to add Task .");
        }
        private async Task EnsureUserHasPermissionToUpdateTask(int TasID , int UserIDOfRequest)
        {
            Permission? per = await GetPermissionOfTask(TasID,UserIDOfRequest);
            if (per == null || per == Permission.Member)
                throw new UnauthorizedAccessException("You do not have permission to add Task .");
        }
        private async Task EnsureUserHasPermissionToDeleteTask(int TasID, int UserIDOfRequest)
        {
            Permission? per = await GetPermissionOfTask(TasID, UserIDOfRequest);
            if (per == null || per == Permission.Member)
                throw new UnauthorizedAccessException("You do not have permission to Delete Task .");
        }
        private async Task EnsureUserHasPermissionToResetStartDateTask(int TasID, int UserIDOfRequest)
        {
            Permission? per = await GetPermissionOfTask(TasID, UserIDOfRequest);
            if (per == null || per == Permission.Member)
                throw new UnauthorizedAccessException("You do not have permission to Reset StartDate Task.");
        }
        private async Task EnsureUserHasPermissionToResetDeliveryDateTask(int TasID, int UserIDOfRequest)
        {
            Permission? per = await GetPermissionOfTask(TasID, UserIDOfRequest);
            if (per == null || per == Permission.Member)
                throw new UnauthorizedAccessException("You do not have permission to Reset Delivery Date Task.");
        }
        private async Task EnsureUserHasPermissionToGetTask(int TasID, int UserIDOfRequest)
        {
            Permission? per = await GetPermissionOfTask(TasID, UserIDOfRequest);
            if (per == null)
                throw new UnauthorizedAccessException("You do not have permission to Get Task.");
        }
        private async Task EnsureUserHasPermissionToSetDeliveryDate(int UserIDOfRequest , int TasID)
        {
            Permission? per = await permissionService.GetPermissionOfTaskMemberByTaskID(UserIDOfRequest,TasID);
            if (per == null || per == Permission.Member)
                throw new UnauthorizedAccessException("You do not have permission to Set Delivery Date.");
        }
        private async Task EnsureUserHasPermissionToSetStartDate(int UserIDOfRequest, int TasID)
        {
            Permission? per = await permissionService.GetPermissionOfTaskMemberByTaskID(UserIDOfRequest, TasID);
            if (per == null || per == Permission.Member)
                throw new UnauthorizedAccessException("You do not have permission to Set Start Date.");
        }
        
    }
}
