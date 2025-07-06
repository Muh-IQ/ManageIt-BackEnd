using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_Tier.DTOs.Task_DTOs
{
    public class TaskDTO
    {
        
        public int TaskID { get; set; }
        public int MembershipStatus { get; set; }
        public string TaskName { get; set; }
    }
}
