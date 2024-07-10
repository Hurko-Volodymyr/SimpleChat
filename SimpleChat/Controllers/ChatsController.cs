using Microsoft.AspNetCore.Mvc;
using SimpleChat.Models.Abstractions.Requests;
using SimpleChat.Models.Abstractions.Services;

namespace SimpleChat.Controllers
{
    [Route("api/chats")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatsController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost("chats")]
        public async Task<IActionResult> CreateChat([FromBody] CreateChatRequest request)
        {
            var chat = await _chatService.CreateChatAsync(request.Title, request.CreatedById);
            return Ok(chat);
        }

        [HttpGet("chats/{id}")]
        public async Task<IActionResult> GetChat(int id)
        {
            var chat = await _chatService.GetChatByIdAsync(id);
            if (chat == null)
            {
                return NotFound();
            }
            return Ok(chat);
        }

        [HttpGet("chats")]
        public async Task<IActionResult> GetAllChats()
        {
            var chats = await _chatService.GetAllChatsAsync();
            return Ok(chats);
        }

        [HttpPut("chats/{id}")]
        public async Task<IActionResult> UpdateChat(int id, [FromBody] UpdateChatRequest request)
        {
            var chat = await _chatService.GetChatByIdAsync(id);
            if (chat == null)
            {
                return NotFound();
            }
            chat.Title = request.Title;
            await _chatService.UpdateChatAsync(chat);
            return Ok(chat);
        }

        [HttpDelete("chats/{id}")]
        public async Task<IActionResult> DeleteChat(int id, [FromBody] int userId)
        {
            try
            {
                await _chatService.DeleteChatAsync(id, userId);
                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
        }
    }
}