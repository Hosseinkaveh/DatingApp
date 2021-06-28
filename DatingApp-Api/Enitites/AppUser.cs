using System;
using System.Collections.Generic;

namespace DatingApp_Api.Enitites
{
    public class AppUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string KnownAs { get; set; }

        public DateTime Create { get; set; } = DateTime.Now;

        public DateTime LastActive { get; set; }

        public string Gender { get; set; }

        public string Interduction { get; set; }

        public string LookingFor { get; set; }

        public string Interests { get; set; }
        public string City { get; set; }

        public string Country { get; set; }

        public ICollection<Photo> Photos { get; set; }

        public ICollection<UserLike> LikedByUser { get; set; } //who has liked the currently user

        public ICollection<UserLike> LikedUser { get; set; } //currently logged in user has like 

        public ICollection<Message> MessagesSent { get; set; }
        public ICollection<Message> MessagesReceived { get; set; }
    }
}