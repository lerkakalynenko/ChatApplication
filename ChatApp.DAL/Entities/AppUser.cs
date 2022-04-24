using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace ChatApp.DAL.Entities
{
    public class AppUser : IdentityUser
    {
        public ICollection<ChatUser> Chats { get; set; }

        public AppUser()
        {
            // Messages = new HashSet<Message>();
        }
        public virtual ICollection<Message> Messages { get; set; } = new HashSet<Message>();

        public string Password { get; set; }
    }
}
