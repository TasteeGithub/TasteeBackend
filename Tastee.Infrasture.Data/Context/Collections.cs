using System;
using System.Collections.Generic;
using URF.Core.EF.Trackable;

namespace Tastee.Infrastucture.Data.Context
{
    public partial class Collections : Entity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Uid { get; set; }
        public bool IsDefault { get; set; }
    }
}
