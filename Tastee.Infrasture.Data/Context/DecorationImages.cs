using System;
using System.Collections.Generic;
using URF.Core.EF.Trackable;

namespace Tastee.Infrastucture.Data.Context
{
    public partial class DecorationImages : Entity
    {
        public string Id { get; set; }
        public string Image { get; set; }
        public string DecorationId { get; set; }
    }
}
