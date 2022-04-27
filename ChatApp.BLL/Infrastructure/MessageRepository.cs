using System;
using System.Threading.Tasks;
using ChatApp.DAL.EF;
using ChatApp.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.BLL.Infrastructure
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationDbContext _context;

        public MessageRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task DeleteMessageFromAll(int id)
        {
            var entity = await _context.Messages.FindAsync(id);
            _context.Messages.Remove(entity);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteOnlyFromUser(int messageId, string userId)
        {
            var entity = await _context.Messages
                .FirstOrDefaultAsync(c => c.Id == messageId);
            entity.DeletedOnlyFromMyChat = true;
            _context.Messages.Update(entity);
            await _context.SaveChangesAsync();

        }

        public async Task UpdateMessage(int id, string newMessage)
        {
            var entity = await _context.Messages
                .FirstOrDefaultAsync(c => c.Id == id);
            entity.Text = newMessage;
            _context.Messages.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<Message> FindMessage(int id)
        {
            var message = await _context.Messages.FindAsync(id);
            return message;
        }

        public Task<Message> ReplyToMessageInGroup(int groupId, int repliedMessage, int newMessage)
        {
            throw new NotImplementedException();
        }

        public Task<Message> ReplyToPrivateChat(string userId, int repliedMessage, int newMessage)
        {
            throw new NotImplementedException();
        }
    }
}
