using System;
using System.Collections.Generic;
using URF.Core.EF.Trackable;

namespace Tastee.Infrastucture.Data.Context
{
    public partial class NotificationMapping: Entity
    {
        public int Id { get; set; }
        public string NotificationId { get; set; }
        public string UserId { get; set; }
        public string BrandId { get; set; }
        public bool Status { get; set; }
    }
}
