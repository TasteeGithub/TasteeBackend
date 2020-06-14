using System.ComponentModel.DataAnnotations;

namespace TTBackEnd.Shared
{
    public class LoginModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
