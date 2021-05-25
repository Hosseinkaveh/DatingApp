using System.ComponentModel.DataAnnotations;

namespace DatingApp_Api.DTOs
{
    public class RegisterDto
    {
        [Required]
          public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}