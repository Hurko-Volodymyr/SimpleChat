using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SimpleChat.Hubs;
using SimpleChat.Models;
using SimpleChat.Models.Abstractions.Requests;
using SimpleChat.Models.Abstractions.Services;
using System.Net;
using System.Net.Http.Json;

namespace SimpleChat.Tests
{
    public class ChatsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ChatsControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task CreateChat_ShouldReturnOk()
        {
            // Arrange
            var chatServiceMock = new Mock<IChatService>();
            chatServiceMock.Setup(service => service.CreateChatAsync(It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(new Chat { ChatId = 999999999, Title = "Test Chat", CreatedById = 1 });

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton(chatServiceMock.Object);
                });
            }).CreateClient();

            var request = new CreateChatRequest { Title = "Test Chat", CreatedById = 1 };

            // Act
            var response = await client.PostAsJsonAsync("/api/chats/chats", request);

            // Assert
            response.EnsureSuccessStatusCode();
            var chat = await response.Content.ReadFromJsonAsync<Chat>();
            Assert.NotNull(chat);
            Assert.Equal("Test Chat", chat.Title);            
        }

        [Fact]
        public async Task GetChat_ShouldReturnOk_WhenChatExists()
        {
            // Arrange
            var chatServiceMock = new Mock<IChatService>();
            chatServiceMock.Setup(service => service.GetChatByIdAsync(1))
                .ReturnsAsync(new Chat { ChatId = 1, Title = "Test Chat", CreatedById = 1 });

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton(chatServiceMock.Object);
                });
            }).CreateClient();

            // Act
            var response = await client.GetAsync("/api/chats/chats/1");

            // Assert
            response.EnsureSuccessStatusCode();
            var chat = await response.Content.ReadFromJsonAsync<Chat>();
            Assert.NotNull(chat);
            Assert.Equal("Test Chat", chat.Title);
        }

        [Fact]
        public async Task GetChat_ShouldReturnNotFound_WhenChatDoesNotExist()
        {
            // Arrange
            var chatServiceMock = new Mock<IChatService>();
            chatServiceMock.Setup(service => service.GetChatByIdAsync(1))
                .ReturnsAsync((Chat)null);

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton(chatServiceMock.Object);
                });
            }).CreateClient();

            // Act
            var response = await client.GetAsync("/api/chats/chats/1");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);


        }

        [Fact]
        public async Task DeleteChat_ShouldReturnNoContent_WhenUserIsOwner()
        {
            // Arrange
            var chatServiceMock = new Mock<IChatService>();
            chatServiceMock.Setup(service => service.DeleteChatAsync(999999999, 1))
                .ReturnsAsync(new List<int> { 1 });

            var hubContextMock = new Mock<IHubContext<ChatHub>>();

            var client = _factory.CreateClient();

            // Act
            var response = await client.DeleteAsync("/api/chats/chats/999999999?userId=1");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            hubContextMock.Verify(
                hub => hub.Clients.Group("1").SendCoreAsync(
                    "DisconnectUsers",
                    It.Is<object[]>(parameters =>
                    parameters.Length == 1 &&
                    parameters[0] is IEnumerable<int>),
                    default(CancellationToken)),
                Times.Once);
        }

        [Fact]
        public async Task DeleteChat_ShouldReturnForbidden_WhenUserIsNotOwner()
        {
            // Arrange
            var client = _factory.CreateClient();
            var userId = 2;
            var idToDelete = 1;

            // Act
            var response = await client.DeleteAsync($"/api/chats/chats/{idToDelete}?userId={userId}");

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}
