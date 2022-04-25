using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ChatApp.BLL.Infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace ChatApp.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {

        private readonly IChatRepository _chatRepository;
        private readonly IUserRepository _userRepository;

        public HomeController(IChatRepository chatRepository, IUserRepository userRepository)
        {
            _chatRepository = chatRepository;
            _userRepository = userRepository;
        }

        // displaying all chats that and weren't joined by a user yet
        public Task<IActionResult> Index()
        {
            var chats =  _chatRepository.GetChats(GetUserId());

            return Task.FromResult<IActionResult>(View(chats));
        }

        // creating new chat and joining as an admin
        [HttpPost]
        public async Task<IActionResult> CreateRoom(string name)
        {

            await _chatRepository.CreateRoom(name, GetUserId());

            return RedirectToAction("Index", "Home");
        }

        // joining defined chat
        [HttpGet("{id}")]
        public async Task<IActionResult> Chat(int id)
        {
            var chat = await _chatRepository.GetChat(id);

            return View(chat);
        }

        // joining the room as a member
        [HttpGet]
        public async Task<IActionResult> JoinRoom(int id)
        {
            await _chatRepository.JoinRoom(id, GetUserId());

            return RedirectToAction("Chat", new { id = id });

        }

        // creating message 
        [HttpPost]
        public async Task<IActionResult> CreateMessage(int chatId, string message)
        {
            try
            {
                await _chatRepository.CreateMessage(chatId, message, GetUserId());
                return RedirectToAction("Chat", new { id = chatId });

            }
            catch
            {
                return BadRequest("Something went wrong");
            }

        }

        // creating the private chat
        public async Task<IActionResult> CreatePrivateRoom(string userId)
        {
           
            var chat = await _chatRepository.GetPrivateChat(GetUserId(), userId);

            if (chat != null)
            {
                return RedirectToAction("Chat", new { chat.Id });
            }

            var id = await _chatRepository.CreatePrivateRoom(GetUserId(), userId);
            
            return RedirectToAction("Chat", new { id });
        }

        // find other users
        public IActionResult Find()
        {
            var users = _userRepository.GetUsers(GetUserId());

            return View(users);
        }



        public IActionResult Privacy()
        {
            return View();
        }

       
    }
}