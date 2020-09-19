using System;
using System.Collections.Generic;
using URF.Core.EF.Trackable;

namespace Tastee.Models.SqlDataContext
{
    public partial class Roles : Entity
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
