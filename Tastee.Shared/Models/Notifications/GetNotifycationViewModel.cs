using System;
using System.Collections.Generic;
using System.Text;

namespace Tastee.Shared.Models.Notifications
{
    public class GetNotifycationViewModel
    {
        public string Draw { get; set; }
        public string Start { get; set; }
        public string Length { get; set; }
        public NotificationType? Type { get; set; }
    }
}
