using System;
using System.ComponentModel.DataAnnotations;

namespace TTFrontEnd.Models
{
    public partial class CreateUserModel
    {

        [Required(ErrorMessage ="Chưa có Email")]
        public string Email { get; set; }
        [Display(Name ="Phone number")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage ="Chưa có tên")]
        [Display(Name = "Full name")]
        public string FullName { get; set; }
        public DateTime? Birthday { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }
        public string Status { get; set; }
    }
}
