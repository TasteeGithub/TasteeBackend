using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Net.Http.Headers;
using Tastee.Application.Interfaces;

namespace Tastee.Application.Services
{
    public class FileService : IFileService
    {
        public string Upload(IFormFile file,string targetFolder)
        {
            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim().ToString();

            FileInfo fileInfo = new FileInfo(fileName);
            string newFileName = Path.GetRandomFileName().Replace(".", string.Empty) + fileInfo.Extension.Replace("\"", string.Empty);

            var fullPath = Path.Combine(targetFolder, newFileName);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return newFileName;
        }
    }
}
