using System;
using System.Collections.Generic;
using System.Text;
using Tastee.Shared;

namespace Tastee.Application.ViewModel
{
    public class GetSuggestBrandsViewModel
    {
        public string Draw { get; set; }
        public string Start { get; set; }
        public string Length { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public SuggestBrandStatus? Status { get; set; }
    }
}
