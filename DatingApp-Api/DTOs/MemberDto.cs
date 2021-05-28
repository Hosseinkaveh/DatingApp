using System;
using System.Collections.Generic;
namespace DatingApp_Api.DTOs
{
    public class MemberDto
    {
          public int Id { get; set; }
        public string userName { get; set; }

        public string PhotoUrl { get; set; }

        public int  Age {get;set;}

        public string KnownAs { get; set; }

        public DateTime Create { get; set; }

        public DateTime LastActive { get; set; }

        public string Gender { get; set; }

        public string Interduction { get; set; }

        public string LookingFor { get; set; }

        public string Interests { get; set; }
        public string City { get; set; }

        public string Country { get; set; }

        public virtual ICollection<PhotoDto> Photos { get; set; }
    }
}