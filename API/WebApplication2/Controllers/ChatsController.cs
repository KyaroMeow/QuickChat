using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using WebApplication2.DTO;
using Microsoft.EntityFrameworkCore;


namespace WebApplication2.Controllers
{
    [Route("api/chats")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly QuickChatBaseContext _context;

        public ChatsController(QuickChatBaseContext context)
        {
            _context = context;
        }

        // GET: api/chats
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChatDTO>>> GetUserChats([FromQuery] int userId)
        {
            return await _context.Chats
                .Where(c => c.Users.Any(u => u.Id == userId))
                .Select(c => new ChatDTO
                {
                    Id = c.Id,
                    UserIds = c.Users.Select(u => u.Id).ToList()
                })
                .ToListAsync();
        }

        // POST: api/chats
        [HttpPost]
        public async Task<ActionResult<ChatDTO>> CreateChat(ChatCreateDTO chatDto)
        {
            var users = await _context.Users
                .Where(u => chatDto.UserIds.Contains(u.Id))
                .ToListAsync();

            var chat = new Chat
            {
                Users = users
            };

            _context.Chats.Add(chat);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserChats), new { userId = users.First().Id }, new ChatDTO
            {
                Id = chat.Id,
                UserIds = chat.Users.Select(u => u.Id).ToList()
            });
        }
    }
}
