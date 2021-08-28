using System;
using System.Collections.Generic;
using System.Text;

namespace Tastee.Domain.Entities 
{
    class RestaurantSpace : BaseEntity
    {
        public string BrandId { get; set; }
        public string Image { get; set; }
        public int? Status { get; set; }
        public string Description { get; set; }
        public long CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public long? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
