using System;
using System.Collections.Generic;
using URF.Core.EF.Trackable;

namespace Tastee.Infrastucture.Data.Context
{
    public partial class ProductSliders: Entity
    {
        public string Id { get; set; }
        public string BrandId { get; set; }
        public int Order { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public string Image { get; set; }
        public string CreatedBy { get; set; }
        public string UpdateBy { get; set; }
    }
}
