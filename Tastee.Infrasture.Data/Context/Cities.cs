using System;
using System.Collections.Generic;

namespace Tastee.Infrastucture.Data.Context
{
    public partial class Cities
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDisabled { get; set; }
    }
}
