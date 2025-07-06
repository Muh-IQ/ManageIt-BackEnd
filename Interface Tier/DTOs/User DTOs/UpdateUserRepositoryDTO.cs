using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_Tier.DTOs.User_DTOs
{
    public class UpdateUserRepositoryDTO
    {
        public int UserID { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string ImagePath { get; set; }
    }
}
