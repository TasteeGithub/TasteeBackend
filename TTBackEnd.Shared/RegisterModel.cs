using System;
using System.ComponentModel.DataAnnotations;

namespace TTBackEnd.Shared
{
    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name ="Full Name")]
        public string FullName { get; set; }

        public string PhoneNumber { get; set; }
        //public DateTime CreatedDate { get; set; }
        public bool IsLocked { get; set; }
        public DateTime? Birthday { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Role { get; set; }
        public int? UserLevel { get; set; }
        public int? MerchantLevel { get; set; }
        public string Avatar { get; set; }
        public string Status { get; set; }

    }

    public class UpdateModel
    {

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        //public DateTime CreatedDate { get; set; }
        public bool IsLocked { get; set; }
        public DateTime? Birthday { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }
        public string Status { get; set; }

    }

}
