using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Text;

namespace Tastee.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UtilitiesController : Controller
    {
        [HttpPost]
        [Route("upload-image")]
        public IActionResult UploadImage()
        {
            try
            {
                var files = Request.Form.Files;
                var folderName = Path.Combine("ClientApp", "public", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (files.Count > 0)
                {
                    StringBuilder newFiles = new StringBuilder();
                    foreach (var file in files)
                    {
                        if (file.Length > 0)
                        {
                            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim().ToString();

                            FileInfo fileInfo = new FileInfo(fileName);
                            string newFileName = Path.GetRandomFileName().Replace(".", string.Empty) + fileInfo.Extension.Replace("\"", string.Empty);

                            var fullPath = Path.Combine(pathToSave, newFileName);
                            using (var stream = new FileStream(fullPath, FileMode.Create))
                            {
                                file.CopyTo(stream);
                            }
                            newFiles.AppendJoin(",",newFileName);
                        }
                    }
                    return Ok(newFiles.ToString());
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}