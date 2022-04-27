using System;
using System.Threading.Tasks;
using ChatApp.BLL.Infrastructure;
using ChatApp.BLL.Infrastructure.Hubs;
using ChatApp.DAL.Entities;
using ChatApp.Models;
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
        private readonly IMessageRepository _messageRepository;


        public ChatController(IHubContext<ChatHub> chat, IChatRepository chatRepository,
            IMessageRepository messageRepository)
        {
            _chat = chat;
            _chatRepository = chatRepository;
            _messageRepository = messageRepository;
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
        public async Task<IActionResult> SendMessage(int chatId, string roomName, string message)
        {
            try
            {
                if (message == null)
                {
                    return RedirectToAction("Chat", "Home", chatId);
                }

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

        [HttpGet("DeleteMessageFromAll")]
        public async Task<IActionResult> DeleteMessageFromAll(int id, int chatId)
        {
            try
            {
                await _messageRepository.DeleteMessageFromAll(id);
            }
            catch
            {
                return BadRequest("Something went wrong");
            }

            return RedirectToAction("Chat", "Home", new {id = chatId});
        }

        [HttpGet("DeleteOnlyFromUser")]
        public async Task<IActionResult> DeleteOnlyFromUser(int id, int chatId)
        {
            try
            {
                await _messageRepository.DeleteOnlyFromUser(id, GetUserId());
            }
            catch
            {
                return BadRequest("Something went wrong");
            }

            return RedirectToAction("Chat", "Home", new {id = chatId});
        }


        [HttpGet("Edit")]
        public async Task<ActionResult> Edit(int id, int chatId)
        {
            try
            {
                var message = await _messageRepository.FindMessage(id);

                if (message != null)
                {
                    var model = new EditViewModel()
                    {
                        MessageId = id,
                        ChatId = chatId,
                        Message = message.Text,
                    };

                    return View(model);
                }
            }
            catch
            {
                return BadRequest("Message doesn't exist");
            }


            return RedirectToAction("Chat", "Home", new { id = chatId });

        }

        [HttpPost("SaveEditModel")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveEditModel(EditViewModel model)
        {
            var chatId = model.ChatId;
            var newMessage = model.Message;
            var messageId = model.MessageId;
            try
            {
                await _messageRepository.UpdateMessage(messageId, newMessage);
            }
            catch
            {
                return BadRequest("Something went wrong");
            }

            return RedirectToAction("Chat", "Home", new { id = chatId });
        }

        [HttpGet("ReplyToGroup")]
        public async Task<IActionResult> ReplyToGroup(int messageId, int chatId, string roomName)
        {
            try
            {
                var message = await _messageRepository.FindMessage(messageId);

                if (message != null)
                { 
                    var model = new ReplyViewModel()
                    {
                       MessageId = messageId,
                       ChatId = chatId,
                       RoomName = roomName,
                       RepliedMessage = message.Text,
                       UserName = message.UserName,
                    };

                    return View(model);
                }
            }
            catch
            {
                return BadRequest("Message doesn't exist");
            }


            return RedirectToAction("Chat", "Home", new { id = chatId });
        }

        [HttpPost("SaveReply")]
        [ValidateAntiForgeryToken]
        public IActionResult SaveReply(ReplyViewModel model)
        {
            int roomId = model.ChatId;
            if (model.Reply != null)
            {
                try
                {
                    int chatId = model.ChatId;
                    string roomName = model.RoomName;
                    string message = $"Replied to {model.UserName} message: {model.RepliedMessage}" +
                                     $"\n {model.Reply}";

                    return RedirectToAction("SendReply", "Chat",
                        new { chatId = chatId, roomName = roomName, message = message });
                }
                catch
                {
                    return BadRequest("Something went wrong");
                }
            }

            return RedirectToAction("Chat", "Home", new {chatId = roomId});
        }

        [HttpGet("SendReply")]
        public async Task<IActionResult> SendReply(int chatId, string roomName, string message)
        {
            try
            {
                if (message == null)
                {
                    return RedirectToAction("Chat", "Home", new{id=chatId});
                }

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
                return RedirectToAction("Chat", "Home", new{id = chatId});
            }
            catch
            {
                return BadRequest("Something went wrong");
            }
        }

    }
}