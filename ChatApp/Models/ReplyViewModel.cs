namespace ChatApp.Models
{
    public class ReplyViewModel
    {
        public int MessageId { get; set; }
        public int ChatId { get; set; }
        public string RoomName { get; set; }
        public string RepliedMessage { get; set; }
        public string Reply { get; set; }
        public string UserName { get; set; }
    }
}
