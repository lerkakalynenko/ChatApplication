using System.Threading.Tasks;
using ChatApp.DAL.Entities;

namespace ChatApp.BLL.Infrastructure
{
    public interface IMessageRepository
    {
        Task DeleteMessageFromAll(int id);
        Task DeleteOnlyFromUser(int messageId, string userId);
        Task UpdateMessage(int id, string newMessage);
        Task<Message> FindMessage(int id);
    }
}
