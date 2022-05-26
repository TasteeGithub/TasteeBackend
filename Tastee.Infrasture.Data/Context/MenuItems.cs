using System;
using System.Collections.Generic;
using URF.Core.EF.Trackable;

namespace Tastee.Infrastucture.Data.Context
{
    public partial class MenuItems: Entity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string MenuId { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public decimal Price { get; set; }
        public int? LikeNumber { get; set; }
        public int? SaleNumber { get; set; }
        public int Status { get; set; }
        public int? Order { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string BrandId { get; set; }
        public decimal? CapitalPrice { get; set; }
        public decimal? WholesalePrice { get; set; }
        public decimal? PromotionPrice { get; set; }
        public bool IsGroupTopping { get; set; }
        public string ProductCode { get; set; }
        public string Barcode { get; set; }
        public bool IsInventory { get; set; }
        public int? TotalInventory { get; set; }
        public string Unit { get; set; }
    }
}
