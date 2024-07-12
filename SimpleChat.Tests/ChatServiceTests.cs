using Moq;
using SimpleChat.Models;
using SimpleChat.Models.Abstractions.Repositories;
using SimpleChat.Services;

namespace SimpleChat.Tests
{
    public class ChatServiceTests
    {
        private readonly Mock<IRepository<Chat>> _mockChatRepository;
        private readonly Mock<IRepository<Message>> _mockMessageRepository;
        private readonly Mock<IRepository<User>> _mockUserRepository;

        public ChatServiceTests()
        {
            _mockChatRepository = new Mock<IRepository<Chat>>();
            _mockMessageRepository = new Mock<IRepository<Message>>();
            _mockUserRepository = new Mock<IRepository<User>>();
        }

        [Fact]
        public async Task CreateChatAsync_ShouldCreateChat()
        {
            // Arrange
            var chatService = new ChatService(_mockChatRepository.Object, _mockMessageRepository.Object, _mockUserRepository.Object);
            var title = "Test Chat";
            var createdById = 1;
            var creator = new User { UserId = createdById, UserName = "Creator" };

            _mockUserRepository.Setup(r => r.GetByIdAsync(createdById)).ReturnsAsync(creator);
            _mockChatRepository.Setup(r => r.AddAsync(It.IsAny<Chat>())).Returns(Task.CompletedTask);
            _mockChatRepository.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var chat = await chatService.CreateChatAsync(title, createdById);

            // Assert
            Assert.NotNull(chat);
            Assert.Equal(title, chat.Title);
            Assert.Equal(createdById, chat.CreatedById);

            _mockUserRepository.Verify(r => r.GetByIdAsync(createdById), Times.Once);
            _mockChatRepository.Verify(r => r.AddAsync(It.IsAny<Chat>()), Times.Once);
            _mockChatRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetChatByIdAsync_ShouldReturnChat()
        {
            // Arrange
            var chatId = 1;
            var expectedChat = new Chat { ChatId = chatId, Title = "Test Chat", CreatedById = 1 };

            _mockChatRepository.Setup(r => r.GetByIdAsync(chatId)).ReturnsAsync(expectedChat);

            var chatService = new ChatService(_mockChatRepository.Object, _mockMessageRepository.Object, _mockUserRepository.Object);

            // Act
            var chat = await chatService.GetChatByIdAsync(chatId);

            // Assert
            Assert.NotNull(chat);
            Assert.Equal(expectedChat.ChatId, chat.ChatId);
            Assert.Equal(expectedChat.Title, chat.Title);
            Assert.Equal(expectedChat.CreatedById, chat.CreatedById);
        }

        [Fact]
        public async Task GetAllChatsAsync_ShouldReturnAllChats()
        {
            // Arrange
            var expectedChats = new List<Chat>
            {
                new Chat { ChatId = 1, Title = "Chat 1", CreatedById = 1 },
                new Chat { ChatId = 2, Title = "Chat 2", CreatedById = 2 }
            };

            _mockChatRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(expectedChats);

            var chatService = new ChatService(_mockChatRepository.Object, _mockMessageRepository.Object, _mockUserRepository.Object);

            // Act
            var chats = await chatService.GetAllChatsAsync();

            // Assert
            Assert.NotNull(chats);
            Assert.Equal(expectedChats.Count, chats.Count());

            foreach (var expectedChat in expectedChats)
            {
                var chat = chats.FirstOrDefault(c => c.ChatId == expectedChat.ChatId);
                Assert.NotNull(chat);
                Assert.Equal(expectedChat.Title, chat.Title);
                Assert.Equal(expectedChat.CreatedById, chat.CreatedById);
            }
        }

        [Fact]
        public async Task UpdateChatAsync_ShouldUpdateChat()
        {
            // Arrange
            var existingChat = new Chat { ChatId = 1, Title = "Existing Chat", CreatedById = 1 };
            var updatedChat = new Chat { ChatId = 1, Title = "Updated Chat", CreatedById = 1 };

            _mockChatRepository.Setup(r => r.UpdateAsync(It.IsAny<Chat>())).Returns(Task.CompletedTask);
            _mockChatRepository.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var chatService = new ChatService(_mockChatRepository.Object, _mockMessageRepository.Object, _mockUserRepository.Object);

            // Act
            var result = await chatService.UpdateChatAsync(updatedChat);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedChat.ChatId, result.ChatId);
            Assert.Equal(updatedChat.Title, result.Title);
            Assert.Equal(updatedChat.CreatedById, result.CreatedById);

            _mockChatRepository.Verify(r => r.UpdateAsync(It.IsAny<Chat>()), Times.Once);
            _mockChatRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteChatAsync_ShouldDeleteChat_WhenUserIsOwner()
        {
            // Arrange
            var chatId = 1;
            var userId = 1;
            var usersInChat = new List<Message>
            {
                new Message { ChatId = chatId, UserId = 1 },
                new Message { ChatId = chatId, UserId = 2 }
            };

            _mockChatRepository.Setup(r => r.GetByIdAsync(chatId)).ReturnsAsync(new Chat { ChatId = chatId, CreatedById = userId });
            _mockChatRepository.Setup(r => r.DeleteAsync(It.IsAny<Chat>())).Returns(Task.CompletedTask);
            _mockChatRepository.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            _mockMessageRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(usersInChat);

            var chatService = new ChatService(_mockChatRepository.Object, _mockMessageRepository.Object, _mockUserRepository.Object);

            // Act
            var result = await chatService.DeleteChatAsync(chatId, userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(usersInChat.Select(m => m.UserId).Distinct(), result);

            _mockChatRepository.Verify(r => r.DeleteAsync(It.IsAny<Chat>()), Times.Once);
            _mockChatRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteChatAsync_ShouldThrowUnauthorizedAccessException_WhenUserIsNotOwner()
        {
            // Arrange
            var chatId = 1;
            var userId = 2;

            _mockChatRepository.Setup(r => r.GetByIdAsync(chatId)).ReturnsAsync(new Chat { ChatId = chatId, CreatedById = 1 });

            var chatService = new ChatService(_mockChatRepository.Object, _mockMessageRepository.Object, _mockUserRepository.Object);

            // Act and Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => chatService.DeleteChatAsync(chatId, userId));

            _mockChatRepository.Verify(r => r.DeleteAsync(It.IsAny<Chat>()), Times.Never);
            _mockChatRepository.Verify(r => r.SaveChangesAsync(), Times.Never);
        }
    }
}
