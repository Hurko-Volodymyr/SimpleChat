using Microsoft.EntityFrameworkCore;
using SimpleChat.Models;

namespace SimpleChat.Data
{
    public class ChatAppContext : DbContext
    {
        public ChatAppContext(DbContextOptions<ChatAppContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.CreatedChats)
                .WithOne(c => c.CreatedBy)
                .HasForeignKey(c => c.CreatedById);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Messages)
                .WithOne(m => m.User)
                .HasForeignKey(m => m.UserId);

            modelBuilder.Entity<Chat>()
                .HasMany(c => c.Messages)
                .WithOne(m => m.Chat)
                .HasForeignKey(m => m.ChatId);

            modelBuilder.Entity<User>().HasData(
                new User { UserId = 1, UserName = "User1" },
                new User { UserId = 2, UserName = "User2" }
            );

            modelBuilder.Entity<Chat>().HasData(
                new Chat { ChatId = 1, Title = "General", CreatedById = 1 },
                new Chat { ChatId = 2, Title = "Random", CreatedById = 2 }
            );
        }
    }

}
