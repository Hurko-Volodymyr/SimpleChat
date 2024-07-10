using Microsoft.EntityFrameworkCore;
using SimpleChat.Data;
using SimpleChat.Models.Abstractions;
using SimpleChat.Models;

namespace SimpleChat.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly ChatAppContext _context;

        public ChatRepository(ChatAppContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Chat>> GetAllChatsAsync()
        {
            return await _context.Chats.ToListAsync();
        }

        public async Task<Chat> GetChatByIdAsync(int id)
        {
            return await _context.Chats.FindAsync(id);
        }

        public async Task CreateChatAsync(Chat chat)
        {
            _context.Chats.Add(chat);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateChatAsync(Chat chat)
        {
            _context.Entry(chat).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteChatAsync(int id)
        {
            var chat = await _context.Chats.FindAsync(id);
            if (chat != null)
            {
                _context.Chats.Remove(chat);
                await _context.SaveChangesAsync();
            }
        }
    }

}
