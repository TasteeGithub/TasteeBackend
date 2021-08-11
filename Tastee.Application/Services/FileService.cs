using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Domain.Models;
using Tastee.Shared;

namespace Tastee.Application.Services
{
    public class FileService : IFileService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<FileService> _logger;

        private readonly TransferUtility _transferUtility;

        public FileService(IConfiguration configuration, ILogger<FileService> logger, TransferUtility transferUtility)
        {
            _configuration = configuration;
            _logger = logger;

            _transferUtility = transferUtility;
        }

        /// <summary>
        /// Uploads to an S3 bucket with specified name
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="file"></param>
        /// <param name="objectTypeName"></param>
        /// <param name="metaTags"></param>
        /// <returns>An AWSUploadResult with the link to the uploaded content, if successful</returns>
        public async Task<AWSUploadResult<string>> UploadImageToS3BucketAsync(IFormFile file, UploadFileType fileType)
        {
            try
            {
                string bucketName = _configuration["AWS:BucketName"];

                if (!IsValidImageFile(file, fileType))
                {
                    _logger.LogInformation("Invalid file");
                    return new AWSUploadResult<string>
                    {
                        Status = false,
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }

                // Rename file to random string to prevent injection and similar security threats
                var trustedFileName = WebUtility.HtmlEncode(file.FileName);
                var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                var randomFileName = Path.GetRandomFileName() + DateTime.UtcNow.ToString("ddMMyyyyHms");
                var uploadPath = fileType == UploadFileType.Image ? _configuration["Path:UploadImagePath"] : _configuration["Path:UploadFilePath"];
                var trustedStorageName = uploadPath + randomFileName + ext;

                // Create the image object to be uploaded in memory
                var transferUtilityRequest = new TransferUtilityUploadRequest()
                {
                    InputStream = file.OpenReadStream(),
                    Key = trustedStorageName,
                    BucketName = bucketName,
                    CannedACL = S3CannedACL.PublicRead, // Ensure the file is read-only to allow users view their pictures
                    PartSize = 6291456
                };

                // Add metatags which can include the original file name and other decriptions
                //var metaTags = requestDto.Metatags;
                //if (metaTags != null && metaTags.Count() > 0)
                //{
                //    foreach (var tag in metaTags)
                //    {
                //        transferUtilityRequest.Metadata.Add(tag.Key, tag.Value);
                //    }
                //}

                transferUtilityRequest.Metadata.Add("originalFileName", trustedFileName);


                await _transferUtility.UploadAsync(transferUtilityRequest);

                // Retrieve Url
                var ImageUrl = GenerateAwsFileUrl(bucketName, trustedStorageName).Data;

                _logger.LogInformation("File uploaded to Amazon S3 bucket successfully");
                return new AWSUploadResult<string>
                {
                    Status = true,
                    Data = ImageUrl
                };
            }
            catch (Exception ex) when (ex is NullReferenceException)
            {
                _logger.LogError("File data not contained in form", ex);
                throw;
            }
            catch (Exception ex) when (ex is AmazonS3Exception)
            {
                _logger.LogError("Something went wrong during file upload", ex);
                throw;
            }

        }

        /// <summary>
        /// Generates URL for uploaded file
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="key"></param>
        /// <param name="useRegion"></param>
        /// <returns></returns>
        public AWSUploadResult<string> GenerateAwsFileUrl(string bucketName, string key, bool useRegion = true)
        {
            // URL patterns: Virtual hosted style and path style
            // Virtual hosted style
            // 1. http://[bucketName].[regionName].amazonaws.com/[key]
            // 2. https://[bucketName].s3.amazonaws.com/[key]

            // Path style: DEPRECATED
            // 3. http://s3.[regionName].amazonaws.com/[bucketName]/[key]
            string publicUrl = string.Empty;
            if (useRegion)
            {
                publicUrl = $"https://{bucketName}.s3.{_configuration["AWS:Region"]}.amazonaws.com/{key}";
            }
            else
            {
                publicUrl = $"https://{bucketName}.amazonaws.com/{key}";
            }
            return new AWSUploadResult<string>
            {
                Status = true,
                Data = publicUrl
            };
        }

        /// <summary>
        /// Checks if an uploaded file matches accepted constraints
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private bool IsValidImageFile(IFormFile file, UploadFileType fileType)
        {
            string[] permittedExtensions;
            // Check file length
            if (file.Length < 0)
            {
                return false;
            }

            // Check file extension to prevent security threats associated with unknown file types
            if (fileType == UploadFileType.File)
            {
                permittedExtensions = new string[] { ".pdf" };
            }
            else
            {
                permittedExtensions = new string[] { ".jpg", ".jpeg", ".png" };
            }
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains<string>(ext))
            {
                return false;
            }

            // Check if file size is greater than permitted limit
            //if (file.Length > _config.FileSize) // 6MB
            //{
            //    return false;
            //}

            return true;
        }

        //public string Upload(IFormFile file,string targetFolder)
        //{
        //    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim().ToString();

        //    FileInfo fileInfo = new FileInfo(fileName);
        //    string newFileName = Path.GetRandomFileName().Replace(".", string.Empty) + fileInfo.Extension.Replace("\"", string.Empty);

        //    var fullPath = Path.Combine(targetFolder, newFileName);
        //    using (var stream = new FileStream(fullPath, FileMode.Create))
        //    {
        //        file.CopyTo(stream);
        //    }
        //    return newFileName;
        //}
    }
}
