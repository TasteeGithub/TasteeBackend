
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tastee.Domain.Models;
using Tastee.Domain.Models.Brands;
using Tastee.Shared;
using Tastee.Shared.Models.Files;

namespace Tastee.Application.Interfaces
{
    public interface IFileService
    {
        Task<AWSUploadResult<string>> UploadFilesToS3BucketAsync(IFormFile file, UploadFileType fileType);
        Task<string> UploadFolderToS3BucketAsync(string directoryPath, string keyPrefix);
        public UploadTmpFolderResponse UploadTmpFolder(List<IFormFile> files);
        bool IsValidFile(IFormFile file, UploadFileType fileType);
        void DeleteFolder(string path, bool deleteContent = true);
        AWSUploadResult<string> GenerateAwsFileUrl(string key, bool useRegion = true);
        Task<string> DeleteFromS3BucketAsync(string url);
        string GenerateS3KeyPrefix(string objectId, UploadFileType fileType = UploadFileType.Image, ObjectType objectType = ObjectType.Brand);
    }
}
