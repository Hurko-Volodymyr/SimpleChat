using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SimpleChat.Controllers;
using SimpleChat.Models;
using SimpleChat.Models.Abstractions;
using SimpleChat.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SimpleChat.Tests
{
    public class ChatsControllerTests
    {

        private readonly Mock<ILogger<ChatsController>> _logger = new Mock<ILogger<ChatsController>>();
        [Fact]
        public async Task CreateChat_ReturnsCreatedResponse()
        {
            // Arrange
            var mockService = new Mock<IChatService>();
            mockService.Setup(repo => repo.CreateChatAsync(It.IsAny<Chat>()))
                .Returns(Task.CompletedTask);

            var controller = new ChatsController(mockService.Object, _logger.Object);

            var newChat = new Chat { Title = "Test Chat", CreatedById = 1 };

            // Act
            var result = await controller.CreateChat(newChat);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var model = Assert.IsType<Chat>(createdResult.Value);
            Assert.Equal("Test Chat", model.Title);
        }

        [Fact]
        public async Task GetChatById_ReturnsOkResult()
        {
            // Arrange
            var mockService = new Mock<IChatService>();
            mockService.Setup(repo => repo.GetChatByIdAsync(1))
                .Returns(Task.FromResult(new Chat { ChatId = 1, Title = "Test Chat", CreatedById = 1 }));

            var controller = new ChatsController(mockService.Object, _logger.Object);

            // Act
            var result = await controller.GetChatById(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Chat>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var model = Assert.IsType<Chat>(okResult.Value);
            Assert.Equal("Test Chat", model.Title);
        }


        [Fact]
        public async Task UpdateChat_ReturnsNoContentResult()
        {
            // Arrange
            var mockService = new Mock<IChatService>();
            mockService.Setup(repo => repo.UpdateChatAsync(It.IsAny<Chat>()))
                .Returns(Task.CompletedTask);

            var controller = new ChatsController(mockService.Object, _logger.Object);

            var updatedChat = new Chat { ChatId = 1, Title = "Updated Chat", CreatedById = 1 };

            // Act
            var result = await controller.UpdateChat(1, updatedChat);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, noContentResult.StatusCode);
        }

        [Fact]
        public async Task UpdateChat_WhenChatNotFound_ReturnsNotFoundResult()
        {
            // Arrange
            var mockService = new Mock<IChatService>();
            mockService.Setup(repo => repo.UpdateChatAsync(It.IsAny<Chat>()))
                .ThrowsAsync(new KeyNotFoundException("Chat not found"));

            var controller = new ChatsController(mockService.Object, _logger.Object);

            var updatedChat = new Chat { ChatId = 1, Title = "Updated Chat", CreatedById = 1 };

            // Act
            var result = await controller.UpdateChat(1, updatedChat);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task DeleteChat_ReturnsNoContentResult()
        {
            // Arrange
            var mockService = new Mock<IChatService>();
            mockService.Setup(repo => repo.DeleteChatAsync(1))
                .Returns(Task.CompletedTask);

            var controller = new ChatsController(mockService.Object, _logger.Object);

            // Act
            var result = await controller.DeleteChat(1);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, noContentResult.StatusCode);
        }

        [Fact]
        public async Task DeleteChat_WhenChatNotFound_ReturnsNotFoundResult()
        {
            // Arrange
            var mockService = new Mock<IChatService>();
            mockService.Setup(repo => repo.DeleteChatAsync(1))
                .ThrowsAsync(new KeyNotFoundException("Chat not found"));

            var controller = new ChatsController(mockService.Object, _logger.Object);

            // Act
            var result = await controller.DeleteChat(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

    }
}