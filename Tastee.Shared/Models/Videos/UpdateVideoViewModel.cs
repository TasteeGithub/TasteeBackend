using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Tastee.Shared.Models.Videos
{
    public class UpdateVideoViewModel
    {
        public IFormFile File { get; set; }
        [Required]
        public string Id { get; set; }
        public string Name { get; set; }
        [Required]
        public int DisplayOrder { get; set; }
        [Required]
        public int Type { get; set; }
        [Required]
        public bool IsDisplay { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Video { get; set; }
    }
}
