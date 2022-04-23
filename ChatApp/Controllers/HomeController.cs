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


        public HomeController(IChatRepository chatRepository)
        {
          
            _chatRepository = chatRepository;
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
        public Task<IActionResult> Chat(int id)
        {
            var chat = _chatRepository.GetChat(id);
            return Task.FromResult<IActionResult>(View(chat));
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
        public IActionResult Privacy()
        {
            return View();
        }



       
    }
}