using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_Tier.DTOs.Task_DTO
{
    public class UpdateTaskDTO
    {
        public int TaskID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TaskStatusID { get; set; }
    }
}
