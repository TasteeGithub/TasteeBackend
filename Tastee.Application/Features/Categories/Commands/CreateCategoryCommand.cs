using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Application.Utilities;
using Tastee.Shared;
using Tastee.Shared.Models.Categories;
using Tastee.Shared.Models.Files;

namespace Tastee.Application.Features.Categories.Commands
{
    public class CreateCategoryCommand : IRequest<Response>
    {
        public InsertCategoryViewModel Model;
        public string CreateBy;
    }

    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Response>
    {
        private readonly ICategoryService _categoryService;
        private readonly IFileService _fileService;
        private readonly IConfiguration _configuration;

        public CreateCategoryCommandHandler(ICategoryService categoryService, IFileService fileService, IConfiguration configuration)
        {
            _categoryService = categoryService;
            _fileService = fileService;
            _configuration = configuration;
        }

        public async Task<Response> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            if (request.Model.File != null && !_fileService.IsValidFile(request.Model.File, UploadFileType.Image))
            {
                return new UploadFilesResponse { Successful = false, Message = "Invalid file" };
            }
            var category = request.Model.Adapt<Tastee.Infrastucture.Data.Context.Categories>();
            category.CreatedDate = Converters.DateTimeToUnixTimeStamp(DateTime.Now).Value;
            category.CreatedBy = request.CreateBy;
            var response = await _categoryService.InsertAsync(category);
            if (!response.Successful)
                return response;

            if (request.Model.File != null)
            {
                List<IFormFile> files = new List<IFormFile>();
                files.Add(request.Model.File);
                var imgDict = _fileService.UploadTmpFolder(files);
                var keyPrefix = _fileService.GenerateS3KeyPrefix(category.Id, UploadFileType.Image, ObjectType.Category);
                var uploadResult = await _fileService.UploadFolderToS3BucketAsync(imgDict.FolderPath, keyPrefix);
                if (!String.IsNullOrEmpty(uploadResult))
                {
                    return new UploadFilesResponse { Successful = false, Message = uploadResult };
                }
                _fileService.DeleteFolder(imgDict.FolderPath);
                string bucketName = _configuration["AWS:BucketName"];
                var url = _fileService.GenerateAwsFileUrl(String.Format("{0}/{1}", keyPrefix, imgDict.ImgDictionary[0])).Data;
                response = await _categoryService.UpdateImageAsync(category.Id, url);
                if (!response.Successful)
                {
                    response.Message = "Category Added but update image failed";
                    return response;
                }
            }
            return new Response() { Successful = true, Message ="Insert Successful" };
        }
    }
}