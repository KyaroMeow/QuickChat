using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.DTO;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly QuickChatBaseContext _context;

        public UsersController(QuickChatBaseContext context)
        {
            _context = context;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            return await _context.Users
                .Select(u => new UserDTO
                {
                    Id = u.Id,
                    Login = u.Login,
                    Username = u.Username,
                    AvatarUrl = u.Avatarurl,
                    LastOnline = u.Lastonline,
                    IsOnline = u.Isonline ?? false
                })
                .ToListAsync();
        }
    }
}
