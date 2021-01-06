using System.ComponentModel.DataAnnotations;

namespace Tastee.Models.DTO
{
    public class ChangePasswordRequest
    {
        public string Id { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}