using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ChatApp.DAL.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Models
{
    public class RoomViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public RoomViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var userId = HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier).Value;

            var chats = _context.ChatUsers
                .Include(c => c.Chat)
                .Where(c => c.UserId == userId)
                .Select(c => c.Chat)
                .ToList();

            return View(chats);
        }
    }
}
