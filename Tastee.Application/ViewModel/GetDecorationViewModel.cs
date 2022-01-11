using System;
using System.Collections.Generic;
using System.Text;
using Tastee.Shared;

namespace Tastee.Application.ViewModel
{
    public class GetDecorationViewModel
    {
        public string Draw { get; set; }
        public string Start { get; set; }
        public string Length { get; set; }
        public string BrandId { get; set; }
        public string DecorationId { get; set; }
        public BrandDecorationStatus? Status { get; set; }
    }
}
