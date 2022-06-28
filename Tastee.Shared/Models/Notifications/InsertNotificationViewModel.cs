using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Tastee.Shared.Models.Notifications
{
    public class InsertNotificationViewModel
    {
        public IFormFile File { get; set; }

        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public NotificationType Type { get; set; }
        public List<string> SendToIds { get; set; }
        [Required]
        public NotifyPushType PushType { get; set; }
    }
}
