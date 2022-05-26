using System;
using System.Collections.Generic;
using URF.Core.EF.Trackable;

namespace Tastee.Infrastucture.Data.Context
{
    public partial class Brands: Entity
    {
        public string Id { get; set; }
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
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdateBy { get; set; }
        public string MetaDescription { get; set; }
        public string SeoTitle { get; set; }
        public string SeoDescription { get; set; }
        public string SeoImage { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string Cuisines { get; set; }
        public string Categories { get; set; }
        public string MerchantId { get; set; }
        public TimeSpan? OpenTimeA { get; set; }
        public TimeSpan? CloseTimeA { get; set; }
        public TimeSpan? OpenTimeP { get; set; }
        public TimeSpan? CloseTimeP { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public short? Type { get; set; }
        public string WebMap { get; set; }
        public short? Partner { get; set; }
        public short BusinessType { get; set; }
        public string Tax { get; set; }
        public string RefCode { get; set; }
        public bool IsOffline { get; set; }
    }
}
