using System;
namespace Tastee.Domain.Entities
{
    public partial class Operator
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
