using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.DAL.EF;
using ChatApp.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.BLL.Infrastructure
{
    public class ChatRepository : IChatRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<AppUser> _userManager;
        public ChatRepository(ApplicationDbContext db, UserManager<AppUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }


        public async Task<Message> CreateMessage(int chatId, string msgText, string userId)
        {
            var message = new Message
            {
                ChatId = chatId,
                Text = msgText,
                UserName = userId,
                SentTime = DateTime.Now
            };

            _db.Messages.Add(message);
            await _db.SaveChangesAsync();

            return message;
        }

        public async Task CreateRoom(string name, string userId)
        {
            var chat = new Chat
            {
                Name = name,
                Type = ChatType.Room
            };

            chat.Users.Add(new ChatUser
            {
                UserId = userId,
                Role = UserRole.Admin
            });

            _db.Chats.Add(chat);

            await _db.SaveChangesAsync();
        }

        public async Task<int> CreatePrivateRoom(string rootId, string targetId)
        {
            var name1 = _userManager.FindByIdAsync(targetId).Result.UserName;
            var name2 = _userManager.FindByIdAsync(rootId).Result.UserName;

            var chat = new Chat
            {

                Name = $"{name1} and {name2}",
                Type = ChatType.Private,

            };
            
            chat.Users.Add(new ChatUser
            {
                UserId = targetId
            });

            chat.Users.Add(new ChatUser
            {
                UserId = rootId,

            });

            _db.Chats.Add(chat);

            await _db.SaveChangesAsync();

            return chat.Id;
        }

        
        public  Chat GetChat(int id)
        {
            return  _db.Chats
                .Include(x => x.Messages)
                .FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Chat> GetChats(string userId)
        {
            return _db.Chats
                .Include(x => x.Users)
                .Where(x => x.Users.All(y => y.UserId != userId) && x.Type!=ChatType.Private);
        }

        public IEnumerable<Chat> GetPrivateChats(string userId)
        {
            return _db.Chats
                .Include(x => x.Users)
                .ThenInclude(x => x.Chat.Users)
                .Where(x => x.Type == ChatType.Private
                            && x.Users
                                .Any(y => y.UserId == userId));
        }

        public async Task JoinRoom(int chatId, string userId)
        {
            var chatUser = new ChatUser
            {
                ChatId = chatId,
                UserId = userId,
                Role = UserRole.Member
            };

            _db.ChatUsers.Add(chatUser);

            await _db.SaveChangesAsync();
        }
    }
}
