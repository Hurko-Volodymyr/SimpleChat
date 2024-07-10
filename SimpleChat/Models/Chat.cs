namespace SimpleChat.Models
{
    public class Chat
    {
        public int ChatId { get; set; }
        public string Title { get; set; }
        public int CreatedById { get; set; }
        public User CreatedBy { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}