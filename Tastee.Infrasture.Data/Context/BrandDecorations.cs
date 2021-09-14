using System;
using System.Collections.Generic;
using URF.Core.EF.Trackable;

namespace Tastee.Infrastucture.Data.Context
{
    public partial class BrandDecorations: Entity
    {
        public string Id { get; set; }
        public string BrandId { get; set; }
        public long CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public long? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string WidgetsJson { get; set; }
        public int Status { get; set; }
    }
}
