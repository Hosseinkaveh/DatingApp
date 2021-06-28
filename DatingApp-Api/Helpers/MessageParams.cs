namespace DatingApp_Api.Helpers
{
    public class MessageParams : PageInationParams
    {
        public string UserName { get; set; }
        public string Container { get; set; } = "Unread";
    }
}