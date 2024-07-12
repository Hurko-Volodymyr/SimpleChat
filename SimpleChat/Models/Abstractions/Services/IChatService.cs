namespace SimpleChat.Models.Abstractions.Services
{
    public interface IChatService
    {
        Task<Chat> CreateChatAsync(string title, int createdById);
        Task<Chat> GetChatByIdAsync(int id);
        Task<IEnumerable<Chat>> GetAllChatsAsync();
        Task<Chat> UpdateChatAsync(Chat chat);
        Task<IEnumerable<int>> DeleteChatAsync(int id, int userId);
    }

}