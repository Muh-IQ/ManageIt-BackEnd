using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_Tier.DTOs.Project_DTOs
{
    public class UserProjectsPagedDTO
    {
        public int ProjectsID { get; set; }
        public string ProjectsName { get; set; }
        public string ProjectStatuseName { get; set; }
        public int ProjectStatuseID { get; set; }
        public byte Permission { get; set; }
    }
}
