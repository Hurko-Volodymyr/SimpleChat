using SimpleChat.Models;
using SimpleChat.Models.Abstractions.Repositories;
using SimpleChat.Models.Abstractions.Services;

namespace SimpleChat.Services
{
    public class ChatService : IChatService
    {
        private readonly IRepository<Chat> _chatRepository;

        public ChatService(IRepository<Chat> chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public async Task<Chat> CreateChatAsync(string title, int createdById)
        {
            var chat = new Chat { Title = title, CreatedById = createdById };
            await _chatRepository.AddAsync(chat);
            await _chatRepository.SaveChangesAsync();
            return chat;
        }

        public async Task<Chat> GetChatByIdAsync(int id) => await _chatRepository.GetByIdAsync(id);

        public async Task<IEnumerable<Chat>> GetAllChatsAsync() => await _chatRepository.GetAllAsync();

        public async Task<Chat> UpdateChatAsync(Chat chat)
        {
            await _chatRepository.UpdateAsync(chat);
            await _chatRepository.SaveChangesAsync();
            return chat;
        }

        public async Task DeleteChatAsync(int id, int userId)
        {
            var chat = await _chatRepository.GetByIdAsync(id);
            if (chat.CreatedById != userId)
            {
                throw new UnauthorizedAccessException("There are no permissions to do the operation");
            }
            await _chatRepository.DeleteAsync(chat);
            await _chatRepository.SaveChangesAsync();
        }
    }
}
