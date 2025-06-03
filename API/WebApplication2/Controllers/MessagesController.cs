using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.DTO;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly QuickChatBaseContext _context;

        public MessagesController(QuickChatBaseContext context)
        {
            _context = context;
        }

        // GET: api/Messages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDTO>>> GetMessages()
        {
            return await _context.Messages
                .Select(m => new MessageDTO
                {
                    Id = m.Id,
                    ChatId = m.Chatid,
                    SenderId = m.Senderid,
                    Text = m.Text,
                    SentAt = m.Sentat ?? DateTime.UtcNow,
                    IsRead = m.Isread ?? false
                })
                .ToListAsync();
        }

        // GET: api/Messages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MessageDTO>> GetMessage(int id)
        {
            var message = await _context.Messages.FindAsync(id);

            if (message == null)
            {
                return NotFound();
            }

            return new MessageDTO
            {
                Id = message.Id,
                ChatId = message.Chatid,
                SenderId = message.Senderid,
                Text = message.Text,
                SentAt = message.Sentat ?? DateTime.UtcNow,
                IsRead = message.Isread ?? false
            };
        }

        // GET: api/Messages/chat/5
        [HttpGet("chat/{chatId}")]
        public async Task<ActionResult<IEnumerable<MessageDTO>>> GetMessagesForChat(int chatId)
        {
            return await _context.Messages
                .Where(m => m.Chatid == chatId)
                .Select(m => new MessageDTO
                {
                    Id = m.Id,
                    ChatId = m.Chatid,
                    SenderId = m.Senderid,
                    Text = m.Text,
                    SentAt = m.Sentat ?? DateTime.UtcNow,
                    IsRead = m.Isread ?? false
                })
                .ToListAsync();
        }

        // PUT: api/Messages/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMessage(int id, MessageUpdateDTO messageDto)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            if (messageDto.IsRead.HasValue)
            {
                message.Isread = messageDto.IsRead;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MessageExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Messages
        [HttpPost]
        public async Task<ActionResult<MessageDTO>> PostMessage(MessageCreateDTO messageDto)
        {
            var message = new Message
            {
                Chatid = messageDto.ChatId,
                Senderid = messageDto.SenderId,
                Text = messageDto.Text,
                Sentat = DateTime.UtcNow,
                Isread = false
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMessage", new { id = message.Id }, new MessageDTO
            {
                Id = message.Id,
                ChatId = message.Chatid,
                SenderId = message.Senderid,
                Text = message.Text,
                SentAt = message.Sentat ?? DateTime.UtcNow,
                IsRead = message.Isread ?? false
            });
        }

        // DELETE: api/Messages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MessageExists(int id)
        {
            return _context.Messages.Any(e => e.Id == id);
        }
    }
}
