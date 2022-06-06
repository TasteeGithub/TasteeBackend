using System;
using System.Collections.Generic;
using URF.Core.EF.Trackable;

namespace Tastee.Infrastucture.Data.Context
{
    public partial class Categories : Entity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int DisplayOrder { get; set; }
        public int Type { get; set; }
        public bool IsDisplay { get; set; }
        public long CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public long? UpdatedDate { get; set; }
        public int? Navigation { get; set; }
    }
}
