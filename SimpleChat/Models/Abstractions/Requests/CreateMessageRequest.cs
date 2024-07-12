namespace SimpleChat.Models.Abstractions.Requests
{
    public class CreateMessageRequest
    {
        public int ChatId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
    }
}
