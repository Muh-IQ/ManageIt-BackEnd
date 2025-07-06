using Interface_Tier.Utiltiy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_Tier.DTOs.Project_Member_DTOs
{
    public class AddProjectMemberServiceDTO
    {
        public int ProjectID { get; set; }
        public string Email { get; set; }
        public Permission Permission { get; set; }
    }
}
