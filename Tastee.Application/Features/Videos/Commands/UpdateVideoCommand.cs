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
using Tastee.Shared.Models.Files;
using Tastee.Shared.Models.Videos;

namespace Tastee.Application.Features.Videos.Commands
{
    public class UpdateVideoCommand : IRequest<Response>
    {
        public UpdateVideoViewModel VideoModel;
        public string UpdateBy;
    }

    public class UpdateVideoCommandHandler : IRequestHandler<UpdateVideoCommand, Response>
    {
        private readonly IVideoService _videoService;
        private readonly IFileService _fileService;

        public UpdateVideoCommandHandler(IVideoService videoService, IFileService fileService)
        {
            _videoService = videoService;
            _fileService = fileService;
        }

        public async Task<Response> Handle(UpdateVideoCommand request, CancellationToken cancellationToken)
        {
            if (request.VideoModel.File != null && !_fileService.IsValidFile(request.VideoModel.File, UploadFileType.Image))
            {
                return new UploadFilesResponse { Successful = false, Message = "Invalid file" };
            }
            var video = request.VideoModel.Adapt<Tastee.Infrastucture.Data.Context.Videos>();
            video.UpdatedBy = request.UpdateBy;

            if (request.VideoModel.File != null)
            {
                List<IFormFile> files = new List<IFormFile>();
                files.Add(request.VideoModel.File);
                var imgDict = _fileService.UploadTmpFolder(files);
                var keyPrefix = _fileService.GenerateS3KeyPrefix(request.VideoModel.Id, UploadFileType.Image, ObjectType.VideoImage);
                var uploadResult = await _fileService.UploadFolderToS3BucketAsync(imgDict.FolderPath, keyPrefix);
                if (!String.IsNullOrEmpty(uploadResult))
                {
                    return new UploadFilesResponse { Successful = false, Message = uploadResult };
                }
                _fileService.DeleteFolder(imgDict.FolderPath);
                var url = _fileService.GenerateAwsFileUrl(String.Format("{0}/{1}", keyPrefix, imgDict.ImgDictionary[0])).Data;
                video.Image = url;
            }

            return await _videoService.UpdateAsync(video);
        }
    }
}