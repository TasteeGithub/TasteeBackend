using System;
using System.Collections.Generic;
using System.Text;

namespace Tastee.Shared.Models.GroupItems
{
    public class UpdateGroupToppingViewModel
    {
        public string Id { get; set; }
        public string BrandId { get; set; }
        public string Name { get; set; }
        public CommonStatus? Status { get; set; }
        public string MenuItemId { get; set; }
        public int? DisplayOrder { get; set; }
        public bool IsRequired { get; set; }
        public int? Min { get; set; }
        public int? Max { get; set; }
    }
}
