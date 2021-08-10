using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Tastee.Domain.Models
{
    public class UploadRequestDto
    {
        [Required]
        public IFormFile File { get; set; }

        [Required]
        public bool IsImage { get; set; }

    }
}
