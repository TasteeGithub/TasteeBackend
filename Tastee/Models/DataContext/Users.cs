using System;
using System.Collections.Generic;
using URF.Core.EF.Trackable;

namespace TTFrontEnd.Models.DataContext
{
    public partial class Users : Entity
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
        public decimal? LoginFailedCount { get; set; }
        public bool? IsMerchant { get; set; }
    }
}
