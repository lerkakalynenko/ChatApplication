using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.DAL.EF;
using ChatApp.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.BLL.Infrastructure
{
    public class ChatRepository : IChatRepository
    {
        private readonly ApplicationDbContext _db;

        public ChatRepository(ApplicationDbContext db)
        {
            _db = db;
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

        public async Task<int> CreatePrivateRoom(string rootId, string targetId)
        {
            var chat = new Chat
            {
                Type = ChatType.Private
            };
            
            chat.Users.Add(new ChatUser
            {
                UserId = targetId
            });

            chat.Users.Add(new ChatUser
            {
                UserId = rootId
            });

            _db.Chats.Add(chat);

            await _db.SaveChangesAsync();

            return chat.Id;
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

        public Chat GetChat(int id)
        {
            return _db.Chats
                .Include(x => x.Messages)
                .FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Chat> GetChats(string userId)
        {
            return _db.Chats
                .Include(x => x.Users)
                .Where(x => x.Users.All(y => y.UserId != userId));
            //TODO: если вдруг каким-то чудом тут будет ошибка - вернуть ToList()
        }

        public IEnumerable<Chat> GetPrivateChats(string userId)
        {
            return _db.Chats
                .Include(x => x.Users)
                .ThenInclude(x => x.AppUser)
                .Where(x => x.Type == ChatType.Private
                            && x.Users
                                .Any(y => y.UserId == userId));//TODO: и тут
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
