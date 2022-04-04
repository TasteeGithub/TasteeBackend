using System;
using System.Collections.Generic;
using URF.Core.EF.Trackable;

namespace Tastee.Infrastucture.Data.Context
{
    public partial class GroupToppings : Entity
    {
        public string Id { get; set; }
        public string BrandId { get; set; }
        public string MenuItemId { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public int? DisplayOrder { get; set; }
        public bool IsRequired { get; set; }
        public int? Min { get; set; }
        public int? Max { get; set; }
        public long CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public long? UpdatedDate { get; set; }
    }
}
