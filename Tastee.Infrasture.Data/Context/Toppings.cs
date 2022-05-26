using System;
using System.Collections.Generic;
using URF.Core.EF.Trackable;

namespace Tastee.Infrastucture.Data.Context
{
    public partial class Toppings: Entity
    {
        public string Id { get; set; }
        public string GroupToppingId { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public int? OrderBy { get; set; }
        public bool? IsRequired { get; set; }
        public int Price { get; set; }
        public int? Max { get; set; }
        public int? Min { get; set; }
        public bool IsSelected { get; set; }
        public string Note { get; set; }
        public long CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public long? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public int? CapitalPrice { get; set; }
        public string BrandId { get; set; }
    }
}
