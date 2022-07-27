using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Tastee.Shared.Models.Banners
{
    public class SetBannerViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Link { get; set; }
        [Required]
        public IFormFile File { get; set; }
        [Required]
        public long StartDate { get; set; }
        [Required]
        public long EndDate { get; set; }

        public string Note { get; set; }
        public string BrandId { get; set; }
        public string Title { get; set; }
        [Required]
        public int DisplayOrder { get; set; }
        [Required]
        public int Type { get; set; }
        [Required]
        public int Navigation { get; set; }
        [Required]
        public bool IsDisplay { get; set; }
        public string Description { get; set; }
    }

    public class UpdateBannerViewModel : SetBannerViewModel
    {
        public new IFormFile File { get; set; }
        [Required]
        public string Id { get; set; }
        [Required]
        public string Status { get; set; }
    }
}
