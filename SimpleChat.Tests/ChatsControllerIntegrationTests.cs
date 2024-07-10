using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using SimpleChat.Models;
using SimpleChat.Models.Abstractions;
using SimpleChat.Services;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleChat.Tests.IntegrationTests
{
    public class ChatsControllerIntegrationTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public ChatsControllerIntegrationTests()
        {
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<TestStartup>());
            _client = _server.CreateClient();
        }

        [Fact]
        public async Task GetAllChats_ReturnsSuccessStatusCode()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/chats");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task CreateChat_ReturnsCreatedStatusCode()
        {
            // Arrange
            var chat = new Chat { Title = "Test Chat", CreatedById = 1 };
            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(chat), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/chats")
            {
                Content = content
            };

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task UpdateChat_ReturnsNoContentStatusCode()
        {
            // Arrange
            var chat = new Chat { ChatId = 1, Title = "Updated Chat", CreatedById = 1 };
            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(chat), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Put, $"/api/chats/{chat.ChatId}")
            {
                Content = content
            };

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task DeleteChat_ReturnsNoContentStatusCode()
        {
            // Arrange
            var chatId = 1;
            var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/chats/{chatId}");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }

    public class TestStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IChatService, MockChatService>();
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    public class MockChatService : IChatService
    {
        public Task<IEnumerable<Chat>> GetAllChatsAsync()
        {
            var chats = new List<Chat>
            {
                new Chat { ChatId = 1, Title = "Chat 1", CreatedById = 1 },
                new Chat { ChatId = 2, Title = "Chat 2", CreatedById = 2 }
            };
            return Task.FromResult<IEnumerable<Chat>>(chats);
        }

        public Task<Chat> GetChatByIdAsync(int id)
        {
            if (id == 1)
            {
                return Task.FromResult(new Chat { ChatId = 1, Title = "Chat 1", CreatedById = 1 });
            }
            else
            {
                return Task.FromResult<Chat>(null);
            }
        }

        public Task CreateChatAsync(Chat chat)
        {
            return Task.CompletedTask;
        }

        public Task UpdateChatAsync(Chat chat)
        {
            return Task.CompletedTask;
        }

        public Task DeleteChatAsync(int id)
        {
            return Task.CompletedTask;
        }
    }
}
