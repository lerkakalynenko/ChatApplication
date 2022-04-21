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

        public string GetConnectionId()
        {
           return Context.ConnectionId;
        }


    }
}
