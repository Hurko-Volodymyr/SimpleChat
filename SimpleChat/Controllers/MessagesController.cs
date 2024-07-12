using Microsoft.AspNetCore.Mvc;
using SimpleChat.Services;
using SimpleChat.Models;
using SimpleChat.Models.Abstractions.Services;
using SimpleChat.Models.Abstractions.Requests;

namespace SimpleChat.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet("{messageId}")]
        public async Task<ActionResult<Message>> GetMessageById(int messageId)
        {
            var message = await _messageService.GetMessageByIdAsync(messageId);
            if (message == null)
            {
                return NotFound();
            }
            return Ok(message);
        }

        [HttpGet("chat/{chatId}")]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessagesByChatId(int chatId)
        {
            var messages = await _messageService.GetMessagesByChatIdAsync(chatId);
            return Ok(messages);
        }

        [HttpPost]
        public async Task<ActionResult<Message>> CreateMessage([FromBody] CreateMessageRequest messageRequest)
        {
            var message = await _messageService.CreateMessageAsync(messageRequest.ChatId, messageRequest.UserId, messageRequest.Content);
            return CreatedAtAction(nameof(GetMessageById), new { messageId = message.MessageId }, message);
        }
    }
}
