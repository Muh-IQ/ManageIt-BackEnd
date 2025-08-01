﻿using Interface_Tier.Utiltiy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_Tier.DTOs.Project_Chat_Member_DTOs
{
    public class ProjectChatMemberPageDTO
    {
        public int ChatMemberID { get; set; }
        public int UserID { get; set; }
        public Permission Permission { get; set; }
        public string UserName { get; set; }
        public string ImagePath { get; set; }
    }
}
