using System;
using System.Collections.Generic;
using System.Text;

namespace Tastee.Shared.Models.Notifications
{
    public class NotificationModel
    {
        public string Id { get; set; }
        public int Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public long CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public long? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public bool SendAll { get; set; }
        public int NotifyType { get; set; }
        public List<string> SendToIds { get; set; }
    }
}
