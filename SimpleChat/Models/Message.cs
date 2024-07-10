namespace SimpleChat.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public string Text { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int ChatId { get; set; }
        public Chat Chat { get; set; }
    }
}