using System;
using System.Collections.Generic;
using System.Text;

namespace Tastee.Domain.Entities
{
    public partial class Menu : BaseEntity
    {
        public string Name { get; set; }
        public string BrandId { get; set; }
        public int Status { get; set; }
        public int Order { get; set; }
        public long CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public long? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
