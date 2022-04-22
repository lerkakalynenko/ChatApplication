using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.DAL.Entities
{
    public class Chat
    {
        public Chat()
        {
            Messages = new List<Message>();
            Users = new List<ChatUser>();
        }

        public int Id { get; set; }

        public ICollection<Message> Messages { get; set; }

        public ICollection<ChatUser> Users { get; set; }

        public ChatType Type { get; set; }

        public string Name { get; set; }
    }

    
}
