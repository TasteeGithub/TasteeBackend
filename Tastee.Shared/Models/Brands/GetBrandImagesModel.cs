using System;
using System.Collections.Generic;
using System.Text;

namespace Tastee.Shared.Models.Brands
{
    public class GetBrandImagesModel: GetCommonModel
    {
        public string ID { get; set; }
        public string BrandID { get; set; }
        public int? Status { get; set; }
    }
}
