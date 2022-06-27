using System;
using System.Collections.Generic;
using System.Text;

namespace Tastee.Shared.Models.Notifications.Firebase
{
    public class FirebaseConfig
    {
        public string type { get; set; }
        public string project_id { get; set; }
        public string client_email { get; set; }
        public string client_id { get; set; }
        public string private_key { get; set; }
    }
}
