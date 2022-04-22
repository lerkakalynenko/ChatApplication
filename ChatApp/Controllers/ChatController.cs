using System;
using System.Threading.Tasks;
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
    public class ChatController : Controller
    {
        private readonly IHubContext<ChatHub> _chat;

        public ChatController(IHubContext<ChatHub> chat)
        {
            _chat = chat;
        }

        [HttpPost("[action]/{connectionId}/{roomName}")]
        public async Task<IActionResult> JoinRoom(string connectionId, string roomName)
        {
            await _chat.Groups.AddToGroupAsync(connectionId, roomName);
            return Ok();
        }

        [HttpPost("[action]/{connectionId}/{roomName}")]
        public async Task<IActionResult> LeaveRoom(string connectionId, string roomName)
        {
            await _chat.Groups.RemoveFromGroupAsync(connectionId, roomName);
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SendMessage(
            int chatId,
            string message,
            string roomName,

            [FromServices] ApplicationDbContext context)
        {
            try
            {
                var Message = new Message
                {
                    ChatId = chatId,
                    Text = message,
                    UserName = User.Identity.Name,
                    When = DateTime.Now,
                };

                await context.Messages.AddAsync(Message);
                await context.SaveChangesAsync();

                await _chat.Clients.Group(roomName)
                    .SendAsync("ReceiveMessage", new
                    {
                        UserName = Message.UserName,
                        Text = Message.Text,
                        When = Message.When.ToString(),
                    });
                return Ok();
            }
            catch
            {
                return BadRequest("something went wrong");
            }
        }




    }
}