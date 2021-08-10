
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Tastee.Domain.Models;
using Tastee.Shared;

namespace Tastee.Application.Interfaces
{
    public interface IFileService
    {
        Task<AWSUploadResult<string>> UploadImageToS3BucketAsync(IFormFile file, UploadFileType fileType);
    }
}
