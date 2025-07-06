using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_Tier.DTOs.Project_Chat_Message_DTOs
{
    public class ProjectChatMessagePageDTOs
    {
        public int ProjectChatMessageID { get; set; }
        public int UserID { get; set; }
        public string Message { get; set; }
        public DateTime SentAt { get; set; }
    }
}
