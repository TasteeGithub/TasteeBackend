using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
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
                foreach (var fileModel in requestModel.Files)
                {
                    if (!_fileService.IsValidFile(fileModel.File, UploadFileType.Image))
                    {
                        return new UploadFilesResponse { Successful = false, Message = "Invalid file" };
                    }
                }

                var brand = await _brandService.GetByIdAsync(requestModel.BrandID);
                if (brand == null)
                {
                    return new UploadFilesResponse { Successful = false, Message = "Brand not found" };
                }

                var imgDict = _fileService.UploadTmpFolder(requestModel);
                var keyPrefix = String.Format("{0}{1}{2}", _configuration["Path:UploadImagePath"], _configuration["Path:UploadBrandImagePath"], requestModel.BrandID);
                var uploadResult = await _fileService.UploadFolderToS3BucketAsync(imgDict.FolderPath, keyPrefix);
                if (!String.IsNullOrEmpty(uploadResult))
                {
                    return new UploadFilesResponse { Successful = false, Message = uploadResult };
                }
                _fileService.DeleteFolder(imgDict.FolderPath);

                var lstRestaurauntSpace = new List<RestaurantSpace>();
                string bucketName = _configuration["AWS:BucketName"];
                var listUrls = new List<string>();
                foreach (var key in imgDict.ImgDictionary.Keys)
                {
                    var url = _fileService.GenerateAwsFileUrl(bucketName, String.Format("{0}/{1}", keyPrefix, imgDict.ImgDictionary[key])).Data;
                    listUrls.Add(url);
                    lstRestaurauntSpace.Add(new RestaurantSpace()
                    {
                        BrandId = requestModel.BrandID,
                        CreatedBy = request.UploadBy,
                        CreatedDate = (long)(Converters.DateTimeToUnixTimeStamp(DateTime.Now).Value),
                        Description = requestModel.Files[key].Description,
                        Id = Guid.NewGuid().ToString(),
                        Image = url
                    });
                }
                await _brandService.InsertRangeRestaurantSpaceAsync(lstRestaurauntSpace);
                return new UploadFilesResponse { Successful = true, Message = "Upload success", Urls = listUrls };
            }
        }
    }
}
