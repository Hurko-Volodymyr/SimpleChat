using SimpleChat.Models;
using SimpleChat.Models.Abstractions.Repositories;
using SimpleChat.Models.Abstractions.Services;

namespace SimpleChat.Services
{
    public class ChatService : IChatService
    {
        private readonly IRepository<Chat> _chatRepository;
        private readonly IRepository<Message> _messageRepository;
        private readonly IRepository<User> _userRepository;

        public ChatService(IRepository<Chat> chatRepository, IRepository<Message> messageRepository, IRepository<User> userRepository)
        {
            _chatRepository = chatRepository;
            _messageRepository = messageRepository;
            _userRepository = userRepository;
        }

        public async Task<Chat> CreateChatAsync(string title, int createdById)
        {
            var creator = await _userRepository.GetByIdAsync(createdById);
            var chat = new Chat
            {
                Title = title,
                CreatedById = createdById,
                CreatedBy = creator,
                Members = new List<User> { creator }
            };

            await _chatRepository.AddAsync(chat);
            await _chatRepository.SaveChangesAsync();

            return chat;
        }

        public async Task<Chat> GetChatByIdAsync(int id)
        {
            var chat = await _chatRepository.GetByIdAsync(id);

            if (chat != null)
            {
                await LoadChatRelations(chat);
            }

            return chat;
        }

        public async Task<IEnumerable<Chat>> GetAllChatsAsync()
        {
            var chats = await _chatRepository.GetAllAsync();

            foreach (var chat in chats)
            {
                await LoadChatRelations(chat);
            }

            return chats;
        }

        private async Task LoadChatRelations(Chat chat)
        {
            await _chatRepository.LoadCollectionAsync(chat, c => c.Members);
            await _chatRepository.LoadCollectionAsync(chat, c => c.Messages);
            await _chatRepository.LoadReferenceAsync(chat, c => c.CreatedBy);
        }


        public async Task<Chat> UpdateChatAsync(Chat chat)
        {
            await _chatRepository.UpdateAsync(chat);
            await _chatRepository.SaveChangesAsync();
            return chat;
        }

        public async Task<IEnumerable<int>> DeleteChatAsync(int id, int userId)
        {
            try
            {
                var chat = await _chatRepository.GetByIdAsync(id);
                if (chat == null)
                {
                    throw new KeyNotFoundException($"Chat with ID '{id}' not found.");
                }

                if (chat.CreatedById != userId)
                {
                    throw new UnauthorizedAccessException("You do not have permissions to delete this chat.");
                }

                var usersInChat = await _messageRepository.GetAllAsync();
                var userIds = usersInChat.Where(m => m.ChatId == id).Select(m => m.UserId).Distinct().ToList();

                await _chatRepository.DeleteAsync(chat);
                await _chatRepository.SaveChangesAsync();

                return userIds;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (UnauthorizedAccessException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
