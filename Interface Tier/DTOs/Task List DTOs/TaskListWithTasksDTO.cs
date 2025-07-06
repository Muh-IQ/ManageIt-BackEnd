using Interface_Tier.DTOs.Task_DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_Tier.DTOs.Task_List_DTOs
{
    public class TaskListWithTasksDTO
    {
        public int TaskListID { get; set; }
        public string TaskListName { get; set; }
        public List<TaskDTO> Tasks { get; set; } = new();
    }
}
