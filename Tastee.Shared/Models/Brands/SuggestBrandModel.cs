using System;
using System.Collections.Generic;
using System.Text;

namespace Tastee.Shared.Models.Brands
{
    public class SuggestBrandModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string RestaurantImages { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int Status {get;set;}
    }
}
