using System;

namespace Tastee.Domain.Entities
{
    public partial class BannerSimple : BaseEntity
    {
        public string Name { get; set; }
        public string Link { get; set; }
        public string Image { get; set; }
        public long StartDate { get; set; }
        public long EndDate { get; set; }
        public long CreatedDate { get; set; }
        public long? UpdateDate { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public string CreatedBy { get; set; }
        public string UpdateBy { get; set; }
        public string BrandId { get; set; }
        public string Title { get; set; }
        public int Views { get; set; }
        public int DisplayOrder { get; set; }
        public int Type { get; set; }
        public int Navigation { get; set; }
        public bool IsDisplay { get; set; }

    }

    public partial class Banner : BannerSimple
    {
        public string Description { get; set; }
    }
}
