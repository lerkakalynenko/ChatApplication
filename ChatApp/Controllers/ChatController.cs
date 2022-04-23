﻿using System;
using System.Threading.Tasks;
using ChatApp.BLL.Infrastructure;
using ChatApp.BLL.Infrastructure.Hubs;
using ChatApp.DAL.EF;
using ChatApp.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ChatController : BaseController
    {
        private readonly IHubContext<ChatHub> _chat;
        private readonly IChatRepository _chatRepository;


        public ChatController(IHubContext<ChatHub> chat, IChatRepository chatRepository)
        {
            _chat = chat;
            _chatRepository = chatRepository;
        }
       

        [HttpPost("[action]/{connectionId}/{chatId}")]
        public async Task<IActionResult> JoinRoom(string connectionId, string chatId)
        {
            await _chat.Groups.AddToGroupAsync(connectionId, chatId);
            return Ok();
        }

        [HttpPost("[action]/{connectionId}/{chatId}")]
        public async Task<IActionResult> LeaveRoom(string connectionId, string chatId)
        {
            await _chat.Groups.RemoveFromGroupAsync(connectionId, chatId);
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SendMessage(
            int chatId,
            string roomName,
            string message,

            [FromServices] ApplicationDbContext context)
        {
            try
            {
                var entity = new Message
                {
                    ChatId = chatId,
                    Text = message,
                    UserName = User.Identity.Name,
                    SentTime = DateTime.Now,
                };

                await _chatRepository.CreateMessage(chatId, message, User.Identity.Name);

                await _chat.Clients.Group(roomName)
                    .SendAsync("ReceiveMessage", new
                    {
                        UserName = entity.UserName,
                        Text = entity.Text,
                        SentTime = entity.SentTime.ToString(),
                    });
                return Ok();
            }
            catch
            {
                return BadRequest("Something went wrong");
            }
        }
    }
}