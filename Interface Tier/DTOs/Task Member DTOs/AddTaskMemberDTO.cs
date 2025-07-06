using Interface_Tier.Utiltiy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_Tier.DTOs
{
    public class AddTaskMemberDTO
    {
        public int TaskID { get; set; }
        public int ProjectMemberID { get; set; }
        public Permission permission { get; set; }
    }
}
