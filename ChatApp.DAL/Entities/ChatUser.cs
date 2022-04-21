using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.Models;

namespace ChatApp.DAL.Entities
{
    public class ChatUser
    {
        public string UserId { get; set; }

        public AppUser AppUser { get; set; }


        public int ChatId { get; set; }

        public Chat Chat { get; set; }

        public UserRole Role { get; set; }
    }
}
