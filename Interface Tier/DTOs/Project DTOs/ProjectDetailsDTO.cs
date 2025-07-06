using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_Tier.DTOs.Project_DTOs
{
    public class ProjectDetailsDTO
    {
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public string Requirements { get; set; }
        public string ProjectStatuseName { get; set; }
        public int ProjectStatusID { get; set; }
    }
}
