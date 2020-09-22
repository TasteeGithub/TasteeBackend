using System;

namespace Tastee.Domain.Models
{
    public partial class Users
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public string FullName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? Birthday { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }
        public DateTime? LastLogin { get; set; }
        public string Status { get; set; }
        public string Qrcode { get; set; }
        public int? LoginFailedCount { get; set; }
        public bool? IsMerchant { get; set; }
    }
}
