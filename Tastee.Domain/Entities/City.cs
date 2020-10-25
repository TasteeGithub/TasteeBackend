using System;

namespace Tastee.Domain.Entities
{
    public partial class City 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDisabled { get; set; }
    }
}
