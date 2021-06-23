namespace DatingApp_Api.Helpers
{
    public class UserParams : PageInationParams
    {

        public string CurrentUserName { get; set; }
        public string Gender { get; set; }

        public int MinAge { get; set; } = 18;
        public int MaxAge { get; set; } = 99;

        public string OrderBy { get; set; } = "lastActive";






    }
}