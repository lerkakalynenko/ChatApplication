using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace Chat.Models
{
    public class AppUser : IdentityUser
    {
        public ICollection<ChatUser> Chats { get; set; }

        public AppUser()
        {
            Messages = new HashSet<Message>();
        }
        public virtual ICollection<Message> Messages { get; set; }

        public string Password { get; set; }
    }
}
