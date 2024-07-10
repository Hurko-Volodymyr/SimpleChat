using System;

namespace SimpleChat.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public ICollection<Chat> CreatedChats { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}
