using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Tastee.Application.Interfaces;

namespace Tastee.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UtilitiesController : Controller
    {
        private readonly IFileService _fileService;
        private readonly ILogger<BrandsController> _logger;
        private readonly IConfiguration _configuration;
        public UtilitiesController(
            ILogger<BrandsController> logger,
            IFileService fileService,
            IConfiguration configuration
            )
        {
            _fileService = fileService;
            _logger = logger;
            _configuration = configuration;
        }
        /// <summary>
        /// Upload hinh
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("upload-image")]
        public IActionResult UploadImage()
        {
            try
            {
                var files = Request.Form.Files;
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), _configuration["Path:UploadImagePath"]);
                if (files.Count > 0)
                {
                    StringBuilder newFiles = new StringBuilder();
                    List<string> listFile = new List<string>();
                    foreach (var file in files)
                    {
                        if (file.Length > 0)
                        {
                            string newFileName = _fileService.Upload(file, pathToSave);
                            listFile.Add(newFileName);
                        }
                    }
                    newFiles.AppendJoin(",",listFile.ToArray());
                    
                    return Ok(newFiles.ToString());
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UploadImage");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}