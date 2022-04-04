using System;
using System.Collections.Generic;
using System.Text;

namespace Tastee.Shared.Models.Topping
{
    public class GetGroupToppingViewModel
    {
        public string Draw { get; set; }
        public string Start { get; set; }
        public string Length { get; set; }
        public string Name { get; set; }
        public string BrandId { get; set; }
        public string MenuItemId { get; set; }
        public CommonStatus? Status { get; set; }
    }
}
