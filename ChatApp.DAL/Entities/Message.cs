﻿using System;
using System.ComponentModel.DataAnnotations;

namespace ChatApp.DAL.Entities
{
    public class Message
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Text { get; set; }

        public DateTime SentTime { get; set; }

        public int ChatId { get; set; }

        public Chat Chat { get; set; }

        public Message()
        {
            SentTime=DateTime.Now;
        }
    }
}
