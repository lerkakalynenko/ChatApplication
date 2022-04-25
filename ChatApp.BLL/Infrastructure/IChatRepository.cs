using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.DAL.Entities;

namespace ChatApp.BLL.Infrastructure
{
    public interface IChatRepository
    {
        Task<Chat> GetChat(int id);
        Task CreateRoom(string name, string userId);
        Task JoinRoom(int chatId, string userId);
        IEnumerable<Chat> GetChats(string userId);
        Task<int> CreatePrivateRoom(string rootId, string targetId);
        Task<Chat> GetPrivateChat(string user1Id, string user2Id);
        Task<Message> CreateMessage(int chatId, string message, string userId);
    }
}
