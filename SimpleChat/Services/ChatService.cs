using SimpleChat.Models;
using SimpleChat.Models.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleChat.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;

        public ChatService(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public async Task<IEnumerable<Chat>> GetAllChatsAsync()
        {
            return await _chatRepository.GetAllChatsAsync();
        }

        public async Task<Chat> GetChatByIdAsync(int id)
        {
            var chat = await _chatRepository.GetChatByIdAsync(id);
            if (chat == null)
            {
                throw new KeyNotFoundException($"Chat with ID {id} not found.");
            }
            return chat;
        }

        public async Task CreateChatAsync(Chat chat)
        {
            await _chatRepository.CreateChatAsync(chat);
        }

        public async Task UpdateChatAsync(Chat chat)
        {
            var existingChat = await _chatRepository.GetChatByIdAsync(chat.ChatId);
            if (existingChat == null)
            {
                throw new KeyNotFoundException($"Chat with ID {chat.ChatId} not found.");
            }

            await _chatRepository.UpdateChatAsync(chat);
        }

        public async Task DeleteChatAsync(int id)
        {
            var existingChat = await _chatRepository.GetChatByIdAsync(id);
            if (existingChat == null)
            {
                throw new KeyNotFoundException($"Chat with ID {id} not found.");
            }

            await _chatRepository.DeleteChatAsync(id);
        }
    }
}
