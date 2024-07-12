using SimpleChat.Models;
using SimpleChat.Models.Abstractions.Repositories;
using SimpleChat.Models.Abstractions.Services;

namespace SimpleChat.Services
{
    public class MessageService : IMessageService
    {
        private readonly IRepository<Message> _messageRepository;
        private readonly IRepository<Chat> _chatRepository;
        private readonly IRepository<User> _userRepository;

        public MessageService(IRepository<Message> messageRepository, IRepository<Chat> chatRepository, IRepository<User> userRepository)
        {
            _messageRepository = messageRepository;
            _chatRepository = chatRepository;
            _userRepository = userRepository;
        }

        public async Task<Message> GetMessageByIdAsync(int messageId)
        {
            return await _messageRepository.GetByIdAsync(messageId);
        }

        public async Task<IEnumerable<Message>> GetMessagesByChatIdAsync(int chatId)
        {
            var chat = await _chatRepository.GetByIdAsync(chatId);
            if (chat == null) throw new Exception("Chat not found");

            await _chatRepository.LoadCollectionAsync(chat, c => c.Messages);
            return chat.Messages;
        }

        public async Task<Message> CreateMessageAsync(int chatId, int userId, string content)
        {
            var chat = await _chatRepository.GetByIdAsync(chatId);
            if (chat == null) throw new Exception("Chat not found");

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new Exception("User not found");

            var message = new Message
            {
                ChatId = chatId,
                UserId = userId,
                Text = content
            };

            await _messageRepository.AddAsync(message);
            await _messageRepository.SaveChangesAsync();

            return message;
        }
    }
}
