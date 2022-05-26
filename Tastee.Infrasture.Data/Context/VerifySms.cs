using System;
using System.Collections.Generic;
using URF.Core.EF.Trackable;

namespace Tastee.Infrastucture.Data.Context
{
    public partial class VerifySms: Entity
    {
        public string Id { get; set; }
        public string VerifyCode { get; set; }
        public string ExtraData { get; set; }
        public int Type { get; set; }
        public long CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public string PhoneNumber { get; set; }
    }
}
