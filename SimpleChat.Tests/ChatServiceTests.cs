using Moq;
using SimpleChat.Models.Abstractions.Repositories;
using SimpleChat.Models;
using SimpleChat.Services;

namespace SimpleChat.Tests
{
    public class ChatServiceTests
    {
        private readonly ChatService _chatService;
        private readonly Mock<IRepository<Chat>> _chatRepositoryMock;

        public ChatServiceTests()
        {
            _chatRepositoryMock = new Mock<IRepository<Chat>>();
            _chatService = new ChatService(_chatRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateChat_ShouldReturnChat()
        {
            var chat = await _chatService.CreateChatAsync("Test Chat", 1);
            Assert.NotNull(chat);
            Assert.Equal("Test Chat", chat.Title);
            Assert.Equal(1, chat.CreatedById);
        }
    }

}
