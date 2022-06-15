using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Domain.Entities;
using Tastee.Shared;
using Tastee.Shared.Models.Banners;
using Tastee.Shared.Models.Files;

namespace Tastee.Features.Banners.Commands
{
    public class UpdateBannerCommand : IRequest<Response>
    {
        public UpdateBannerViewModel Model;
        public string UpdateBy;
    }

    public class UpdateBannerCommandHandler : IRequestHandler<UpdateBannerCommand, Response>
    {
        private readonly IBannerService _bannerService;
        private readonly IFileService _fileService;

        public UpdateBannerCommandHandler(IBannerService bannerService, IFileService fileService)
        {
            _bannerService = bannerService;
            _fileService = fileService;
        }

        public async Task<Response> Handle(UpdateBannerCommand request, CancellationToken cancellationToken)
        {
            if (request.Model.File != null && !_fileService.IsValidFile(request.Model.File, UploadFileType.Image))
            {
                return new UploadFilesResponse { Successful = false, Message = "Invalid file" };
            }
            var banner = request.Model.Adapt<Banner>();
            banner.UpdateBy = request.UpdateBy;

            if (request.Model.File != null)
            {
                List<IFormFile> files = new List<IFormFile>();
                files.Add(request.Model.File);
                var imgDict = _fileService.UploadTmpFolder(files);
                var keyPrefix = _fileService.GenerateS3KeyPrefix(request.Model.Id, UploadFileType.Image, ObjectType.Banner);
                var uploadResult = await _fileService.UploadFolderToS3BucketAsync(imgDict.FolderPath, keyPrefix);
                if (!String.IsNullOrEmpty(uploadResult))
                {
                    return new UploadFilesResponse { Successful = false, Message = uploadResult };
                }
                _fileService.DeleteFolder(imgDict.FolderPath);
                var url = _fileService.GenerateAwsFileUrl(String.Format("{0}/{1}", keyPrefix, imgDict.ImgDictionary[0])).Data;
                banner.Image = url;
            }

            return await _bannerService.UpdateAsync(banner);
        }
    }
}