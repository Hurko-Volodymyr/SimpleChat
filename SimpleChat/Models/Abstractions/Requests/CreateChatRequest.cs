namespace SimpleChat.Models.Abstractions.Requests
{
    public class CreateChatRequest
    {
        public string Title { get; set; }
        public int CreatedById { get; set; }
    }
}