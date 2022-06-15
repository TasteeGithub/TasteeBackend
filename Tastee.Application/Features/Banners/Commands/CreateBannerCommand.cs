using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Application.Utilities;
using Tastee.Domain.Entities;
using Tastee.Shared;
using Tastee.Shared.Models.Banners;
using Tastee.Shared.Models.Files;

namespace Tastee.Features.Banners.Commands
{
    public class CreateBannerCommand : IRequest<Response>
    {
        public SetBannerViewModel Model;
        public string CreateBy;
    }

    public class CreateBannerCommandHandler : IRequestHandler<CreateBannerCommand, Response>
    {
        private readonly IBannerService _bannerService;
        private readonly IFileService _fileService;

        public CreateBannerCommandHandler(IBannerService bannerService, IFileService fileService)
        {
            _bannerService = bannerService;
            _fileService = fileService;
        }

        public async Task<Response> Handle(CreateBannerCommand request, CancellationToken cancellationToken)
        {
            if (request.Model.File != null && !_fileService.IsValidFile(request.Model.File, UploadFileType.Image))
            {
                return new UploadFilesResponse { Successful = false, Message = "Invalid file" };
            }
            var banner = request.Model.Adapt<Banner>();
            banner.CreatedDate = Converters.DateTimeToUnixTimeStamp(DateTime.Now).Value;
            banner.CreatedBy = request.CreateBy;
            banner.Image = String.Empty;
            banner.Id = Guid.NewGuid().ToString();
            banner.Status = BannerStatus.Pending.ToString();
            var response = await _bannerService.InsertAsync(banner);
            if (!response.Successful)
                return response;

            if (request.Model.File != null)
            {
                List<IFormFile> files = new List<IFormFile>();
                files.Add(request.Model.File);
                var imgDict = _fileService.UploadTmpFolder(files);
                var keyPrefix = _fileService.GenerateS3KeyPrefix(banner.Id, UploadFileType.Image, ObjectType.Banner);
                var uploadResult = await _fileService.UploadFolderToS3BucketAsync(imgDict.FolderPath, keyPrefix);
                if (!String.IsNullOrEmpty(uploadResult))
                {
                    return new UploadFilesResponse { Successful = false, Message = uploadResult };
                }
                _fileService.DeleteFolder(imgDict.FolderPath);
                var url = _fileService.GenerateAwsFileUrl(String.Format("{0}/{1}", keyPrefix, imgDict.ImgDictionary[0])).Data;
                response = await _bannerService.UpdateImageAsync(banner.Id, url);
                if (!response.Successful)
                {
                    response.Message = "Banner Added but update image failed";
                    return response;
                }
            }
            return new Response() { Successful = true, Message = "Insert Successful" };
        }
    }
}