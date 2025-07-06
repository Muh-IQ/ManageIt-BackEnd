using Interface_Tier.DTOs;
using Interface_Tier.DTOs.Task_DTO;
using Interface_Tier.DTOs.Task_DTOs;
using Interface_Tier.Utiltiy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_Tier.Service
{
    public interface ITaskService
    {
        Task<bool> AddTask(AddTaskDTO dTO);
        Task<Permission?> GetPermissionOfTask(int TaskID, int UserID);
        Task<bool> UpdateTask(int UserIDOfRequest,UpdateTaskDTO dTO);
        Task<IEnumerable<TaskStatusesDTO>> GetTaskStatuses();
        Task<bool> DeleteTask(int UserIDOfRequest, int TaskID);
        Task<bool> ResetStartDateTask(int UserIDOfRequest, int TaskID);
        Task<bool> ResetDeliveryDateTask(int UserIDOfRequest, int TaskID);
        Task<GetTaskDTO> GetTask(int UserIDOfRequest, int TaskID);
        Task<bool> SetDeliveryDateTask(int UserIDOfRequest, int TaskID);
        Task<bool> SetStartDateTask(int UserIDOfRequest,int TaskID);

    }
}
