using Interface_Tier.Utiltiy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_Tier.DTOs.Task_Member_DTOs
{
    public class TaskMemberPageDTO
    {
        public int TaskMemberID { get; set; }
        public Permission permission { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string ImagePath { get; set; }
    }
}
