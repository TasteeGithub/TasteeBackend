using System;
using System.Collections.Generic;
using URF.Core.EF.Trackable;

namespace Tastee.Infrastucture.Data.Context
{
    public partial class BrandImages : Entity
    {
        public string Id { get; set; }
        public string BrandId { get; set; }
        public string Image { get; set; }
        public int? Status { get; set; }
        public string Description { get; set; }
        public long CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public long? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
