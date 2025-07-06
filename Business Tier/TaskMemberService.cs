using Interface_Tier.DTOs;
using Interface_Tier.DTOs.Task_Member_DTOs;
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
    public class TaskMemberService(ITaskMemberRepository repository, ITaskPermissionService service) : ITaskMemberService
    {
        public async Task<bool> AddTaskMember(int UserIDOfRequest, AddTaskMemberDTO dTO)
        {
            EnsureHeNeedRightPermission(dTO.permission);

            await EnsureUserIsNotAlreadyInTaskMember(dTO.TaskID, dTO.ProjectMemberID);

            await EnsureUserHasPermissionToAddTaskMember(dTO.TaskID, UserIDOfRequest);

            return await repository.AddTaskMember(dTO);
        }
        public async Task<bool> ChangeTaskMemberPermission(int UserIDOfRequest, ChangeTaskMemberPermissionDTO dTO)
        {
            EnsureHeNeedRightPermission(dTO.permission);
            await EnsureUserHasPermissionToChangeTaskMemberPermission(UserIDOfRequest,dTO.TaskMemberID);

            return await repository.ChangeTaskMemberPermission(dTO);
        }
        public async Task<bool> IsUserAlreadyInTaskMember(int TaskID, int ProjectMemberID)
        {
            return await repository.IsUserAlreadyInTaskMember(TaskID, ProjectMemberID);
        }
        public async Task<Permission> GetPermissionOfTaskMember(int UserID, int TaskMemberID)
        {
            return await repository.GetPermissionOfTaskMember(UserID, TaskMemberID);
        }
        public async Task<bool> DeleteTaskMember(int UserIDOfRequest,int TaskMemberID)
        {
            await EnsureUserHasPermissionToDeleteTaskMember(UserIDOfRequest, TaskMemberID);
            return await repository.DeleteTaskMember(TaskMemberID);
        }
        public async Task<IEnumerable<TaskMemberPageDTO>> GetTaskMembersPaged(int UserIDOfRequest, int PageNumber, int PageSize, int TaskID)
        {
            await EnsureUserHasPermissionToGetTaskMembers(UserIDOfRequest, TaskID);
            return await repository.GetTaskMembersPaged(PageNumber, PageSize, TaskID);
        }
        public async Task<int> GetCountTaskMembers(int UserIDOfRequest, int TaskID)
        {
            await EnsureUserHasPermissionToGetTaskMembers(UserIDOfRequest,TaskID);
            return await repository.GetCountTaskMembers(TaskID);
        }
        public async Task<Permission?> GetPermissionOfTaskMemberByTaskID(int UserIDOfRequest, int TaskID)
        {
            return await repository.GetPermissionOfTaskMemberByTaskID(UserIDOfRequest, TaskID);
        }





        private async Task EnsureUserHasPermissionToAddTaskMember(int TaskID, int UserIDOfRequest)
        {
            Permission? per = await service.GetPermissionOfTask(TaskID, UserIDOfRequest);
            if (per == null || per == Permission.Member)
                throw new UnauthorizedAccessException("You do not have permission to Add Task Member.");
        }
        private void EnsureHeNeedRightPermission(Permission permission)
        {
            if (permission == Permission.Owner)
                throw new InvalidOperationException("Cannot assign Owner permission.");
        }
        private async Task EnsureUserIsNotAlreadyInTaskMember(int TaskID, int ProjectMemberID)
        {
            if (await IsUserAlreadyInTaskMember(TaskID, ProjectMemberID))
                throw new ArgumentException("This person already exists in this Task");
        }
        private async Task EnsureUserHasPermissionToChangeTaskMemberPermission(int UserIDOfRequest, int TaskMemberID )
        {
            Permission per = await GetPermissionOfTaskMember( UserIDOfRequest, TaskMemberID);
            if (per == Permission.Member)
                throw new UnauthorizedAccessException("You do not have permission to Change Task Member Permission.");
        }
        private async Task EnsureUserHasPermissionToDeleteTaskMember(int UserIDOfRequest, int TaskMemberID)
        {
            Permission per = await GetPermissionOfTaskMember(UserIDOfRequest, TaskMemberID);
            if (per == Permission.Member)
                throw new UnauthorizedAccessException("You do not have permission to Delete Task Member.");
        }
        private async Task EnsureUserHasPermissionToGetTaskMembers(int UserIDOfRequest, int TaskID)
        {
            Permission? per = await service.GetPermissionOfTask(TaskID, UserIDOfRequest);
            if (per == null)
                throw new UnauthorizedAccessException("You do not have permission to Get Task Members.");
        }

    }
}
