using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_Tier.DTOs.Task_Chat_Message_DTOs
{
    public class TaskChatMessagePageDTOs
    {
        public int TaskChatMessageID { get; set; }
        public int UserID { get; set; }
        public string Message { get; set; }
        public DateTime SentAt { get; set; }
    }
}
