using System;

namespace Tastee.Domain.Entities
{
    public partial class Brand  : BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Hotline { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string HeadOffice { get; set; }
        public string Uri { get; set; }
        public string Logo { get; set; }
        public string RestaurantImages { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public string Status { get; set; }
        public long CreatedDate { get; set; }
        public long? UpdatedDate { get; set; }
        public string UpdateBy { get; set; }
        public string MetaDescription { get; set; }
        public string SeoTitle { get; set; }
        public string SeoDescription { get; set; }
        public string SeoImage { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string[] Cuisines { get; set; }
        public string[] Categories { get; set; }
        public string MerchantId { get; set; }
        public string OpenTimeA { get; set; }
        public string CloseTimeA { get; set; }
        public string OpenTimeP { get; set; }
        public string CloseTimeP { get; set; }
        public long? StartDate { get; set; }
        public long? EndDate { get; set; }
        public short? Type { get; set; }
    }
}
