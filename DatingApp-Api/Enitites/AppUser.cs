using System.Collections.Generic;

namespace DatingApp_Api.Enitites
{
    public class AppUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public byte[] Password { get; set; }
        public byte[] PasswordSalt { get; set; }


        public ICollection<Photo> Photos { get; set; }
    }
}