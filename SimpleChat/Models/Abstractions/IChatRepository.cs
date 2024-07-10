using SimpleChat.Models;

namespace SimpleChat.Models.Abstractions
{
    public interface IChatRepository
    {
        Task<IEnumerable<Chat>> GetAllChatsAsync();
        Task<Chat> GetChatByIdAsync(int id);
        Task CreateChatAsync(Chat chat);
        Task UpdateChatAsync(Chat chat);
        Task DeleteChatAsync(int id);
    }
}
