namespace DatingApp_Api.DTOs
{
    public class UserDto
    {
        public string Token { get; set; }
        public string UserName { get; set; }

        public string KnownAs { get; set; }

        public string PhotoUrl { get; set; }

        public string Gender { get; set; }
    }
}