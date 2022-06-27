using System;
using System.Collections.Generic;
using URF.Core.EF.Trackable;

namespace Tastee.Infrastucture.Data.Context
{
    public partial class DeviceTokens: Entity
    {
        public long Id { get; set; }
        public string DeviceToken { get; set; }
        public string UserId { get; set; }
        public bool? AllowPush { get; set; }
    }
}
