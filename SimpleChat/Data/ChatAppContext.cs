using Microsoft.EntityFrameworkCore;
using SimpleChat.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

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
            modelBuilder.Entity<Chat>()
                .HasOne(c => c.CreatedBy)
                .WithMany(u => u.CreatedChats)
                .HasForeignKey(c => c.CreatedById);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.User)
                .WithMany(u => u.Messages)
                .HasForeignKey(m => m.UserId);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Chat)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ChatId);
        }
    }

}
