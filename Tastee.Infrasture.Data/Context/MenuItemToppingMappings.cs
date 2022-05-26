using System;
using System.Collections.Generic;
using URF.Core.EF.Trackable;

namespace Tastee.Infrastucture.Data.Context
{
    public partial class MenuItemToppingMappings: Entity
    {
        public string Id { get; set; }
        public string MenuItemId { get; set; }
        public int Type { get; set; }
        public string RefId { get; set; }
    }
}
