using System;
using System.Collections.Generic;
using System.Text;

namespace Tastee.Shared.Models.Videos
{
    public class GetVideosViewModel
    {
        public string Draw { get; set; }
        public string Start { get; set; }
        public string Length { get; set; }
        public string Name { get; set; }
        public int? Type { get; set; }
        public bool? IsDisplay { get; set; }
    }
}
