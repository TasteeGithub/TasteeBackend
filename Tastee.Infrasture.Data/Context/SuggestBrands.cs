using System;
using System.Collections.Generic;
using URF.Core.EF.Trackable;

namespace Tastee.Infrastucture.Data.Context
{
    public partial class SuggestBrands : Entity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public long CreatedDate { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string RestaurantImages { get; set; }
        public string CreatedBy { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
    }
}
