using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_Tier.DTOs.Project_DTOs
{
    public class UpdateProjectDTO
    {
        public int ProjectsID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Requirements { get; set; }
        public int ProjectStatuseID { get; set; }
    }
}
