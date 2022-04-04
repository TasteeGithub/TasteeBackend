using System;
using System.Collections.Generic;
using System.Text;

namespace Tastee.Shared.Models.Topping
{
    public class GetToppingViewModel
    {
        public string Draw { get; set; }
        public string Start { get; set; }
        public string Length { get; set; }
        public string Name { get; set; }
        public string GroupToppingId { get; set; }
        public CommonStatus? Status { get; set; }
    }
}
