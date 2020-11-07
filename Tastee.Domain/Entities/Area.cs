using System;

namespace Tastee.Domain.Entities
{
    public partial class Area
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDisabled { get; set; }
        public int CityId { get; set; }
    }
}
