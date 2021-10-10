using System;
using System.Collections.Generic;
using URF.Core.EF.Trackable;

namespace Tastee.Infrastucture.Data.Context
{
    public partial class WidgetImages : Entity
    {
        public string Id { get; set; }
        public string Image { get; set; }
        public string WidgetId { get; set; }
        public int Status { get; set; }
    }
}
