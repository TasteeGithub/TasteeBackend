using System;
using System.Collections.Generic;
using URF.Core.EF.Trackable;

namespace Tastee.Infrastucture.Data.Context
{
    public partial class Widgets: Entity
    {
        public string Id { get; set; }
        public string DecorationId { get; set; }
        public int WidgetType { get; set; }
        public string ExtraData { get; set; }
        public int? DisplayOrder { get; set; }
    }
}
