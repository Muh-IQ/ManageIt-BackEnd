using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_Tier.DTOs
{
    public class AddTaskListDTO
    {
        public string Name { get; set; }
        public int ProjectID { get; set; }
        public int CreatedBy { get; set; }
    }
}
