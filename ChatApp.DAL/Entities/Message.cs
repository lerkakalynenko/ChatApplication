using System;
using System.ComponentModel.DataAnnotations;
using Chat.Models;

namespace ChatApp.DAL.Entities
{
    public class Message
    {
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Text { get; set; }

        public DateTime When { get; set; }

        public int ChatId { get; set; }

        public Chat Chat { get; set; }

        public Message()
        {
            When=DateTime.Now;
        }
    }
}
