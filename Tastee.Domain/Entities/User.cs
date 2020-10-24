using System;

namespace Tastee.Domain.Entities
{
    public partial class User : BaseAccount
    {
        public DateTime? Birthday { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }
        public string Qrcode { get; set; }
        public bool? IsMerchant { get; set; }
    }
}
