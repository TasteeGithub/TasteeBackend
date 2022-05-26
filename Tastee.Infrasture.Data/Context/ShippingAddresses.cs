using System;
using System.Collections.Generic;
using URF.Core.EF.Trackable;

namespace Tastee.Infrastucture.Data.Context
{
    public partial class ShippingAddresses: Entity
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public int Status { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
    }
}
