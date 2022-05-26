using System;
using System.Collections.Generic;
using URF.Core.EF.Trackable;

namespace Tastee.Infrastucture.Data.Context
{
    public partial class GroupItemMapping: Entity
    {
        public string GroupId { get; set; }
        public string ItemId { get; set; }
        public string CreatedBy { get; set; }
        public long CreatedDate { get; set; }
    }
}
