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
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers(string? login = null)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(login))
            {
                query = query.Where(u => EF.Functions.ILike(u.Login, $"%{login}%"));
            }

            var users = await query
                .Select(u => new UserDTO
                {
                    Id = u.Id,
                    Login = u.Login,
                    Username = u.Username,
                    Avatar = u.Avatar,
                    LastOnline = u.Lastonline,
                    IsOnline = u.Isonline ?? false
                })
                .ToListAsync();

            return Ok(users);
        }

        // GET: api/users
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            // Поиск пользователя по идентификатору
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                // Если пользователь не найден, возвращаем 404 Not Found
                return NotFound();
            }

            // Преобразование сущности в DTO
            var userDto = new UserDTO
            {
                Id = user.Id,
                Login = user.Login,
                Username = user.Username,
                Avatar = user.Avatar,
                LastOnline = user.Lastonline,
                IsOnline = user.Isonline ?? false
            };

            // Возвращаем пользователя в виде 200 OK
            return Ok(userDto);
        }

        // GET: api/users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDTO userUpdateDto)
        {
            // 1. Находим пользователя в БД
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound($"Пользователь с ID {id} не найден.");
            }

            // 2. Обновляем поля (только те, что не null в DTO)
            if (userUpdateDto.Username != null)
            {
                user.Username = userUpdateDto.Username;
            }

            if (userUpdateDto.Avatar != null)
            {
                user.Avatar = userUpdateDto.Avatar;
            }

            if (userUpdateDto.IsOnline.HasValue)
            {
                user.Isonline = userUpdateDto.IsOnline.Value;
                user.Lastonline = DateTime.UtcNow; // Обновляем время последнего онлайна
            }

            // 3. Сохраняем изменения
            try
            {
                await _context.SaveChangesAsync();
                return NoContent(); // 204 No Content - успешное обновление
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Users.Any(u => u.Id == id))
                {
                    return NotFound();
                }
                throw;
            }
        }

    }
}
