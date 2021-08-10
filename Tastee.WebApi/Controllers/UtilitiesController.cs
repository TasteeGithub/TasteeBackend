using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Domain.Models;
using Tastee.Shared;

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
        /// Upload image/file
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("upload")]
        public async Task<ActionResult> UploadImage([FromForm] UploadRequestDto requestDto)
        {
            try
            {
                //    var files = Request.Form.Files;
                //    if (files.Count > 0)
                //    {
                //        StringBuilder newFiles = new StringBuilder();
                //        List<string> listFile = new List<string>();
                //        foreach (var file in files)
                //        {
                //            if (file.Length > 0)
                //            {
                //                string newFileName = _fileService.Upload(file, pathToSave);
                //                listFile.Add(newFileName);
                //            }
                //        }
                //        newFiles.AppendJoin(",", listFile.ToArray());
                //        return Ok(newFiles.ToString());
                //    }
                //    else
                //    {
                //        return BadRequest();
                //    }

                var result = await _fileService.UploadImageToS3BucketAsync(requestDto.File, requestDto.IsImage ? UploadFileType.Image : UploadFileType.File);
                if (result.StatusCode == StatusCodes.Status200OK)
                {
                    return new JsonResult(new { url= result.Data}) ;
                }
                else return StatusCode(result.StatusCode);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}