using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SimpleChat.Models.Abstractions.Services;
using System.Threading.Tasks;

namespace SimpleChat.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IMessageService _messageService;
        private readonly IChatService _chatService;
        private readonly ILogger<ChatHub> _logger;

        public ChatHub(IMessageService messageService, IChatService chatService, ILogger<ChatHub> logger)
        {
            _messageService = messageService;
            _chatService = chatService;
            _logger = logger;
        }

        public async Task SendMessage(int chatId, int userId, string message)
        {
            _logger.LogInformation($"Received message '{message}' for chat '{chatId}'");
            var chatMessage = await _messageService.AddMessageAsync(chatId, userId, message);
            await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", chatMessage);
        }

        public async Task JoinChat(int chatId, int userId)
        {
            _logger.LogInformation($"User connected to chat '{chatId}'");
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
        }

        public async Task LeaveChat(int chatId, int userId)
        {
            _logger.LogInformation($"User disconnected from chat '{chatId}'");
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId.ToString());
        }

        public async Task DisconnectUsers(int chatId, IEnumerable<int> userIds)
        {
            foreach (var userId in userIds)
            {
                _logger.LogInformation($"User disconnected from chat '{chatId}'");
                await Clients.User(userId.ToString()).SendAsync("Disconnect");
            }
        }
    }
}