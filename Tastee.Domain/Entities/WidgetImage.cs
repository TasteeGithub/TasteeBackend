using System;
using System.Collections.Generic;
using System.Text;

namespace Tastee.Domain.Entities
{
    public class WidgetImage
    {
        public string Id { get; set; }
        public string Image { get; set; }
        public string WidgetId { get; set; }
        public string DecorationId { get; set; }
        public string BrandId { get; set; }
        public int Status { get; set; }
    }
}
