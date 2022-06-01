using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Domain.Entities;
using Tastee.Shared;
using Tastee.Shared.Models.Categories;
using Tastee.Shared.Models.Files;

namespace Tastee.Application.Features.Categories.Commands
{
    public class UpdateCategoryCommand : IRequest<Response>
    {
        public UpdateCategoryViewModel CategoryModel;
        public string UpdateBy;
    }

    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Response>
    {
        private readonly ICategoryService _categoryService;
        private readonly IFileService _fileService;
        private readonly IConfiguration _configuration;

        public UpdateCategoryCommandHandler(ICategoryService categoryService, IFileService fileService, IConfiguration configuration)
        {
            _categoryService = categoryService;
            _fileService = fileService;
            _configuration = configuration;
        }

        public async Task<Response> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            if (request.CategoryModel.File != null && !_fileService.IsValidFile(request.CategoryModel.File, UploadFileType.Image))
            {
                return new UploadFilesResponse { Successful = false, Message = "Invalid file" };
            }
            var category = request.CategoryModel.Adapt<Tastee.Infrastucture.Data.Context.Categories>();
            category.UpdatedBy = request.UpdateBy;

            if (request.CategoryModel.File != null)
            {
                List<IFormFile> files = new List<IFormFile>();
                files.Add(request.CategoryModel.File);
                var imgDict = _fileService.UploadTmpFolder(files);
                var keyPrefix = _fileService.GenerateS3KeyPrefix(request.CategoryModel.Id, UploadFileType.Image, ObjectType.Category);
                var uploadResult = await _fileService.UploadFolderToS3BucketAsync(imgDict.FolderPath, keyPrefix);
                if (!String.IsNullOrEmpty(uploadResult))
                {
                    return new UploadFilesResponse { Successful = false, Message = uploadResult };
                }
                _fileService.DeleteFolder(imgDict.FolderPath);
                string bucketName = _configuration["AWS:BucketName"];
                var url = _fileService.GenerateAwsFileUrl(String.Format("{0}/{1}", keyPrefix, imgDict.ImgDictionary[0])).Data;
                category.Image = url;
            }

            return await _categoryService.UpdateAsync(category);
        }
    }
}