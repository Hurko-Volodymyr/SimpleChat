using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimpleChat.Models;
using SimpleChat.Models.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleChat.Controllers
{
    [Route("api/chats")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly ILogger<ChatsController> _logger;

        public ChatsController(IChatService chatService, ILogger<ChatsController> logger)
        {
            _chatService = chatService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Chat>>> GetAllChats()
        {
            try
            {
                var chats = await _chatService.GetAllChatsAsync();
                return chats == null ? NotFound("No chats found.") : Ok(chats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all chats.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Chat>> CreateChat(Chat chat)
        {
            try
            {
                await _chatService.CreateChatAsync(chat);
                return CreatedAtAction(nameof(GetChatById), new { id = chat.ChatId }, chat);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating chat.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Chat>> GetChatById(int id)
        {
            try
            {
                var chat = await _chatService.GetChatByIdAsync(id);
                if (chat == null)
                {
                    return NotFound();
                }
                return Ok(chat);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting chat by id.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateChat(int id, Chat chat)
        {
            try
            {
                if (id != chat.ChatId)
                {
                    return BadRequest("Chat ID mismatch.");
                }

                await _chatService.UpdateChatAsync(chat);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating chat.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChat(int id)
        {
            try
            {
                await _chatService.DeleteChatAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting chat.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
            }
        }
    }
}