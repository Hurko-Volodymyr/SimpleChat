
namespace SimpleChat.Models.Abstractions.Services
{
    public interface IMessageService
    {
        Task<Message> GetMessageByIdAsync(int messageId);
        Task<Message> CreateMessageAsync(Message messageDto);
        Task<CancellationToken> AddMessageAsync(int chatId, int userId, string message);
    }

}