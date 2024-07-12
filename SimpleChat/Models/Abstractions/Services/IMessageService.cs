
namespace SimpleChat.Models.Abstractions.Services
{
    public interface IMessageService
    {
        Task<Message> GetMessageByIdAsync(int messageId);
        Task<IEnumerable<Message>> GetMessagesByChatIdAsync(int chatId);
        Task<Message> CreateMessageAsync(int chatId, int userId, string content);
    }


}