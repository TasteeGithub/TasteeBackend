using System;
using System.Collections.Generic;
using System.Text;

namespace Tastee.Shared.Models.GroupItems
{
    public class DeleteGroupItemMappingViewModel
    {
        public string GroupId { get; set; }
        public List<string> MenuItemIds { get; set; }
    }
}
