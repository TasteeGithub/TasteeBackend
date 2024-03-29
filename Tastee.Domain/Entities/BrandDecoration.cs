﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Tastee.Domain.Entities
{
    public class BrandDecoration : BaseEntity
    {
        public string BrandId { get; set; }
        public long CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public long? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public int Status { get; set; }
    }
}
