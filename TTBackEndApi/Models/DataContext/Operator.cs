using System;
using System.Collections.Generic;
using URF.Core.EF.Trackable;

namespace TTFrontEnd.Models.DataContext
{
    public partial class Operator : Entity
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Email { get; set; }
    }
}
