using Interface_Tier.DTOs;
using Interface_Tier.DTOs.Task_DTO;
using Interface_Tier.DTOs.Task_DTOs;
using Interface_Tier.Utiltiy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_Tier.Repository
{
    public interface ITaskRepository
    {
        Task<bool> AddTask(AddTaskDTO dTO);
        Task<bool> UpdateTask(UpdateTaskDTO dTO);
        Task<bool> DeleteTask(int TaskID);
        Task<bool> ResetStartDateTask(int TaskID);
        Task<bool> ResetDeliveryDateTask(int TaskID);
        Task<bool> SetStartDateTask(int TaskID);
        Task<bool> SetDeliveryDateTask(int TaskID);
        Task<GetTaskDTO> GetTask(int UserID, int TaskID);
        Task<Permission?> GetPermissionOfTask(int TaskID,int UserID);
        Task<IEnumerable<TaskStatusesDTO>> GetTaskStatuses();
    }
}
