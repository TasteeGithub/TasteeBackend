using System;
using System.Collections.Generic;

namespace Tastee.Infrastucture.Data.Context
{
    public partial class Menus
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string BrandId { get; set; }
        public int Status { get; set; }
        public int Order { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
