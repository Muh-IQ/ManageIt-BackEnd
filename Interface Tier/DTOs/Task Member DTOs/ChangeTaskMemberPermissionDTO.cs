using Interface_Tier.Utiltiy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_Tier.DTOs
{
    public class ChangeTaskMemberPermissionDTO
    {
        public int TaskMemberID { get; set; }
        public Permission permission { get; set; }
    }
}
