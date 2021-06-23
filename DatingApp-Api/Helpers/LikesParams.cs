namespace DatingApp_Api.Helpers
{
    public class LikesParams : PageInationParams
    {
        public int UserId { get; set; }
        public string predicate { get; set; }

    }
}