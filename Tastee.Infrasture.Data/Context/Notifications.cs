using System;
using System.Collections.Generic;
using URF.Core.EF.Trackable;

namespace Tastee.Infrastucture.Data.Context
{
    public partial class Notifications: Entity
    {
        public string Id { get; set; }
        public int Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public long CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public bool SendAll { get; set; }
        public int NotifyType { get; set; }
    }
}
