using System;
using System.Collections.Generic;
using URF.Core.EF.Trackable;

namespace Tastee.Infrastucture.Data.Context
{
    public partial class Cities : Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDisabled { get; set; }
    }
}
