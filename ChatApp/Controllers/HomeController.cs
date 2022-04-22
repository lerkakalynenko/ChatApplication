using ChatApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ChatApp.DAL.EF;
using ChatApp.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Chat = ChatApp.DAL.Entities.Chat;

namespace ChatApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var chats = await _context.Chats
                .Include(c => c.Users)
                .Where(c => c.Users.All(x => x.UserId != userId))
                .ToListAsync();

            return View(chats);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom(string name)
        {
            var chat = new Chat
            {
                Name = name,
                Type = ChatType.Room,
            };

            chat.Users.Add(new ChatUser
            {
                UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                Role = UserRole.Admin,
            });

            await _context.Chats.AddAsync(chat);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Chat(int id)
        {
            var chat = await _context.Chats
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.Id == id);
            return View(chat);
        }

        [HttpGet]
        public async Task<IActionResult> JoinRoom(int id)
        {
            var chatUser = new ChatUser
            {
                ChatId = id,
                UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                Role = UserRole.Member,
            };


            await _context.ChatUsers.AddAsync(chatUser);
            await _context.SaveChangesAsync();

            return RedirectToAction("Chat", new { id = id });
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(int chatId, string message)
        {
            try
            {
                var Message = new Message
                {
                    ChatId = chatId,
                    Text = message,
                    UserName = User.Identity.Name,
                    SentTime = DateTime.Now,
                };
                await _context.Messages.AddAsync(Message);
                await _context.SaveChangesAsync();

                return RedirectToAction("Chat", new { id = chatId });
                
            }
            catch
            {
                return BadRequest("Something went wrong");
            }


        }

        public IActionResult Privacy()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}