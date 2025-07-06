using Interface_Tier.Utiltiy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_Tier.DTOs.Task_DTOs
{
    public class GetTaskDTO
    {
        public string TasksName { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public int TaskStatusID { get; set; }
        public string StatusName { get; set; }
        public Permission? permission { get; set; }
    }
}
