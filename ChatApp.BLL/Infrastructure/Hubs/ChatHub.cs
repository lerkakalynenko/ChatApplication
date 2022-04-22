using System.Threading.Tasks;
using Chat.Models;
using ChatApp.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.BLL.Infrastructure.Hubs
{   
    [Authorize]
    public class ChatHub : Hub
    {
        public string GetConnectionId() => Context.ConnectionId;
       
        public Task JoinRoom(string roomId)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        }

        public Task LeaveRoom(string roomId)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
        }



    }
}
