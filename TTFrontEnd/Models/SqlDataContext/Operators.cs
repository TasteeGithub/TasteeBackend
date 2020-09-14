using System;
using System.Collections.Generic;
using URF.Core.EF.Trackable;

namespace TTFrontEnd.Models.SqlDataContext
{
    public partial class Operators : Entity
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public string FullName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastLogin { get; set; }
        public string Status { get; set; }
        public int? LoginFailedCount { get; set; }
    }
}
