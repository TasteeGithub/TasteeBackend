using System;
using System.Collections.Generic;

namespace Tastee.Infrastucture.Data.Context
{
    public partial class Areas : URF.Core.EF.Trackable.Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDisabled { get; set; }
        public int CityId { get; set; }
    }
}
