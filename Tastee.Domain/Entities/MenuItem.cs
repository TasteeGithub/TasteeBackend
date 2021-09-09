using System;
using System.Collections.Generic;
using System.Text;

namespace Tastee.Domain.Entities
{
    public partial class MenuItem : BaseEntity
    {
        public string Name { get; set; }
        public string MenuId { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public decimal? Price { get; set; }
        public int? LikeNumber { get; set; }
        public int? SaleNumber { get; set; }
        public int? Status { get; set; }
        public int? Order { get; set; }
        public long CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public long? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
