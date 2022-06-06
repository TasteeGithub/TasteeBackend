using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Tastee.Shared.Models.Categories
{
    public class InsertCategoryViewModel
    {
        public IFormFile File { get; set; }
        [Required]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int DisplayOrder { get; set; }
        [Required]
        public int Type { get; set; }
        [Required]
        public bool IsDisplay { get; set; }
        public int? Navigation { get; set; }
    }
}
