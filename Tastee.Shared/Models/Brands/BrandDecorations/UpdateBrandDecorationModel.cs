using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Tastee.Shared.Models.Brands.BrandDecorations
{
    public class UpdateBrandDecorationModel
    {
        public List<FormImageDecoration> Files { get; set; }

        [Required]
        public string BrandID { get; set; }
        [Required]
        public string Widgets { get; set; }
    }

    public class FormImageDecoration
    {
        [Required]
        public IFormFile File { get; set; }
        [Required]
        public string Name { get; set; }

    }
}
