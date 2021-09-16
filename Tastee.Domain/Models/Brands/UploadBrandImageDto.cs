using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Tastee.Domain.Models.Brands
{
    public class UploadBrandImageDto
    {
        [Required]
        public List<BrandFormImage> Files { get; set; }

        [Required]
        public string BrandID { get; set; }
    }

    public class BrandFormImage
    {
        [Required]
        public IFormFile File { get; set; }
        public string Description { get; set; }
        public int? Status { get; set; }

    }
}
