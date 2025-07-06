using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_Tier.DTOs
{
    public class AddTaskDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public int CreatedBy { get; set; }
        public int TaskListID { get; set; }
    }
}
