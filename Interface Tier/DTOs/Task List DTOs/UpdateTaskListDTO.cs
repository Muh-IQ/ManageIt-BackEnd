using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_Tier.DTOs.Task_List_DTOs
{
    public class UpdateTaskListDTO
    {
        public string Name { get; set; }
        public int ProjectID { get; set; }
        public int TaskListID { get; set; }
    }
}
