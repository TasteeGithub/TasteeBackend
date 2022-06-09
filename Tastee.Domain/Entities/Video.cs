using System;
using System.Collections.Generic;
using System.Text;

namespace Tastee.Domain.Entities
{
    public partial class VideoModel : BaseEntity
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Video { get; set; }
        public int Views { get; set; }
        public int DisplayOrder { get; set; }
        public int Type { get; set; }
        public bool IsDisplay { get; set; }
        public long CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public long? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
