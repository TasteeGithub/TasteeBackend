using System;
using System.Collections.Generic;
using URF.Core.EF.Trackable;

namespace Tastee.Infrastucture.Data.Context
{
    public partial class Banners: Entity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Image { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public string CreatedBy { get; set; }
        public string UpdateBy { get; set; }
        public string BrandId { get; set; }
        public long StartDate { get; set; }
        public long EndDate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Views { get; set; }
        public int DisplayOrder { get; set; }
        public int Type { get; set; }
        public int Navigation { get; set; }
        public bool IsDisplay { get; set; }
        public long CreatedDate { get; set; }
        public long? UpdateDate { get; set; }
    }
}
