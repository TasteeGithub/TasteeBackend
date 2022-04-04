using System;
using System.Collections.Generic;
using System.Text;
using Tastee.Shared;

namespace Tastee.Domain.Entities
{
    public class GroupTopping : BaseEntity
    {
        public string BrandId { get; set; }
        public long CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public long? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string Name { get; set; }
        public string MenuItemId { get; set; }
        public CommonStatus Status { get; set; }
        public int? DisplayOrder { get; set; }
        public bool IsRequired { get; set; }
        public int? Min { get; set; }
        public int? Max { get; set; }
    }
}