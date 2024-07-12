using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.SignalR.Client;

namespace SimpleChat.Tests
{
    public class ChatHubIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HubConnection _client;

        public ChatHubIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = new HubConnectionBuilder()
          .WithUrl("http://localhost/chathub", options =>
          {
              options.HttpMessageHandlerFactory = _ => _factory.Server.CreateHandler();
          })
          .Build();
        }

        [Fact]
        public async Task AddToChat_ShouldAddUserToChat()
        {
            // Arrange
            var client = new HubConnectionBuilder()
                .WithUrl("http://localhost/chathub", options =>
                {
                    options.HttpMessageHandlerFactory = _ => _factory.Server.CreateHandler();
                })
                .Build();

            var user2Connected = false;

            client.On<string>("GroupAdded", (groupName) =>
            {
                if (groupName == "General")
                {
                    user2Connected = true;
                }
            });

            await client.StartAsync();

            // Act
            await client.InvokeAsync("AddToChat", 1); // Adding user to chat with ChatId 1 (General)

            // Assert
            await Task.Delay(1000); // Wait a bit for events to be processed
            Assert.True(user2Connected, "User should be added to General chat.");

            await client.StopAsync();
        }




        //[Fact]
        //public async Task AddToChat_ShouldAddClientToGroup()
        //{
        //    // Arrange
        //    var client = new HubConnectionBuilder()
        //        .WithUrl("http://localhost/chathub", options =>
        //        {
        //            options.HttpMessageHandlerFactory = _ => _factory.Server.CreateHandler();
        //        })
        //        .Build();

        //    var groupAdded = false;
        //    client.On<string>("GroupAdded", (groupName) =>
        //    {
        //        if (groupName == "1")
        //        {
        //            groupAdded = true;
        //        }
        //    });

        //    await client.StartAsync();

        //    // Act
        //    await client.InvokeAsync("AddToChat", 1);

        //    // Assert
        //    await Task.Delay(1000); // Wait a bit for the event to be processed
        //    Assert.True(groupAdded, "GroupAdded event should have been raised.");

        //    await client.StopAsync();
        //}


        //[Fact]
        //    public async Task RemoveFromChat_ShouldRemoveClientFromGroup()
        //    {
        //        // Arrange
        //        var client = new HubConnectionBuilder()
        //            .WithUrl("http://localhost/chathub", options =>
        //            {
        //                options.HttpMessageHandlerFactory = _ => _factory.Server.CreateHandler();
        //            })
        //            .Build();

        //        var groupRemoved = false;
        //        client.On<string>("GroupRemoved", (groupName) =>
        //        {
        //            if (groupName == "1")
        //            {
        //                groupRemoved = true;
        //            }
        //        });

        //        await client.StartAsync();
        //        await client.InvokeAsync("AddToChat", 1);

        //        // Act
        //        await client.InvokeAsync("RemoveFromChat", 1);

        //        // Assert
        //        await Task.Delay(1000);
        //        Assert.True(groupRemoved);

        //        await client.StopAsync();
        //    }
    }

}
