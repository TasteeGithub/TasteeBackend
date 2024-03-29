﻿using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Application.Utilities;
using Tastee.Domain.Models.Brands;
using Tastee.Infrastucture.Data.Context;
using Tastee.Shared;
using Tastee.Shared.Models.Files;

namespace Tastee.Application.Features.Brands.Commands
{
    public class UploadBrandImagesCommand : IRequest<UploadFilesResponse>
    {
        public UploadBrandImageDto RequestModel;
        public string UploadBy;
        public class UploadBrandImagesCommandHandler : IRequestHandler<UploadBrandImagesCommand, UploadFilesResponse>
        {
            private readonly IFileService _fileService;
            private readonly IBrandService _brandService;
            private readonly IConfiguration _configuration;
            

            public UploadBrandImagesCommandHandler(IFileService fileService, IBrandService brandService, IConfiguration configuration)
            {
                _fileService = fileService;
                _brandService = brandService;
                _configuration = configuration;
            }

            public async Task<UploadFilesResponse> Handle(UploadBrandImagesCommand request, CancellationToken cancellationToken)
            {
                var requestModel = request.RequestModel;
                if(requestModel.Files.Count == 0)
                {
                    return new UploadFilesResponse { Successful = false, Message = "Please input upload files" };
                }

                foreach (var fileModel in requestModel.Files)
                {
                    if (fileModel.File == null || !_fileService.IsValidFile(fileModel.File, UploadFileType.Image))
                    {
                        return new UploadFilesResponse { Successful = false, Message = "Invalid file" };
                    }
                }

                var brand = await _brandService.GetByIdAsync(requestModel.BrandID);
                if (brand == null)
                {
                    return new UploadFilesResponse { Successful = false, Message = "Brand not found" };
                }

                var imgDict = _fileService.UploadTmpFolder(requestModel.Files.Select(x=>x.File).ToList());
                var keyPrefix = _fileService.GenerateS3KeyPrefix(requestModel.BrandID, UploadFileType.Image, ObjectType.Brand);
                var uploadResult = await _fileService.UploadFolderToS3BucketAsync(imgDict.FolderPath, keyPrefix);
                if (!String.IsNullOrEmpty(uploadResult))
                {
                    return new UploadFilesResponse { Successful = false, Message = uploadResult };
                }
                _fileService.DeleteFolder(imgDict.FolderPath);

                var lstRestaurauntSpace = new List<BrandImages>();
                string bucketName = _configuration["AWS:BucketName"];
                var listUrls = new List<string>();
                foreach (var key in imgDict.ImgDictionary.Keys)
                {
                    var url = _fileService.GenerateAwsFileUrl(String.Format("{0}/{1}", keyPrefix, imgDict.ImgDictionary[key])).Data;
                    listUrls.Add(url);
                    lstRestaurauntSpace.Add(new BrandImages()
                    {
                        BrandId = requestModel.BrandID,
                        CreatedBy = request.UploadBy,
                        CreatedDate = (long)(Converters.DateTimeToUnixTimeStamp(DateTime.Now).Value),
                        Description = requestModel.Files[key].Description,
                        Id = Guid.NewGuid().ToString(),
                        Image = url,
                        Status= requestModel.Files[key].Status
                    });
                }
                await _brandService.InsertBrandImagesAsync(lstRestaurauntSpace);
                return new UploadFilesResponse { Successful = true, Message = "Upload success", Urls = listUrls };
            }
        }
    }
}
