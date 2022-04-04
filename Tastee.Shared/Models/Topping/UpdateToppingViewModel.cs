using System;
using System.Collections.Generic;
using System.Text;

namespace Tastee.Shared.Models.GroupItems
{
    public class UpdateToppingViewModel
    {
        public string Id { get; set; }
        public string GroupToppingId { get; set; }
        public string Name { get; set; }
        public CommonStatus Status { get; set; }
        public int? OrderBy { get; set; }
        public bool IsRequired { get; set; }
        public int Price { get; set; }
        public int? Max { get; set; }
        public int? Min { get; set; }
        public bool IsSelected { get; set; }
        public string Note { get; set; }
    }
}
