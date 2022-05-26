using System;
using System.Collections.Generic;
using URF.Core.EF.Trackable;

namespace Tastee.Infrastucture.Data.Context
{
    public partial class BrandMerchants: Entity
    {
        public string Id { get; set; }
        public string BrandId { get; set; }
        public string UserId { get; set; }
        public bool IsDefault { get; set; }
    }
}
