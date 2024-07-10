using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SimpleChat.Tests
{
    public class ChatHubIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ChatHubIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task CanSendMessage()
        {
            // Arrange
            var client = _factory.CreateDefaultClient();
            var serverUri = new Uri(client.BaseAddress, "/chathub");
            var connection = new HubConnectionBuilder()
                .WithUrl(serverUri.ToString())
                .Build();

            string receivedMessage = null;

            connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                receivedMessage = message;
            });

            await connection.StartAsync();

            // Act
            await connection.InvokeAsync("SendMessage", "testUser", "Hello, World!");

            // Assert
            await Task.Delay(1000);
            Assert.Equal("Hello, World!", receivedMessage);

            await connection.StopAsync();
        }
    }
}
