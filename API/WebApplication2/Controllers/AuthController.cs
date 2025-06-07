using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using WebApplication2.DTO;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly QuickChatBaseContext _context;

        public AuthController(QuickChatBaseContext context)
        {
            _context = context;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(UserCreateDTO userDto)
        {
            if (await _context.Users.AnyAsync(u => u.Login == userDto.Login))
            {
                return Conflict("User with this login already exists");
            }

            var user = new User
            {
                Login = userDto.Login,
                Username = userDto.Username,
                Passwordhash = HashPassword(userDto.Password),
                Isonline = true,
                Avatar = userDto.Avatar
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Register), new UserDTO
            {
                Id = user.Id,
                Login = user.Login,
                Username = user.Username,
                IsOnline = user.Isonline ?? false,
                Avatar = user.Avatar
            });
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(UserLoginDTO loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == loginDto.Login);
            if (user == null)
            {
                return Unauthorized("Invalid login or password");
            }

            if (!VerifyPassword(loginDto.Password, user.Passwordhash))
            {
                return Unauthorized("Invalid login or password");
            }

            user.Isonline = true;
            await _context.SaveChangesAsync();

            return Ok(new UserDTO
            {
                Id = user.Id,
                Login = user.Login,
                Username = user.Username,
                LastOnline = user.Lastonline,
                IsOnline = user.Isonline ?? false,
                Avatar = user.Avatar
            });
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private static bool VerifyPassword(string password, string storedHash)
        {
            return HashPassword(password) == storedHash;
        }
    }
}