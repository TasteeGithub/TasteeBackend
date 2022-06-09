using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Application.Utilities;
using Tastee.Shared;
using Tastee.Shared.Models.Files;
using Tastee.Shared.Models.Videos;

namespace Tastee.Application.Features.Videos.Commands
{
    public class CreateVideoCommand : IRequest<Response>
    {
        public InsertVideoViewModel Model;
        public string CreateBy;
    }

    public class CreateVideoCommandHandler : IRequestHandler<CreateVideoCommand, Response>
    {
        private readonly IVideoService _videoService;
        private readonly IFileService _fileService;

        public CreateVideoCommandHandler(IVideoService videoService, IFileService fileService)
        {
            _videoService = videoService;
            _fileService = fileService;
        }

        public async Task<Response> Handle(CreateVideoCommand request, CancellationToken cancellationToken)
        {
            if (request.Model.File != null && !_fileService.IsValidFile(request.Model.File, UploadFileType.Image))
            {
                return new UploadFilesResponse { Successful = false, Message = "Invalid file" };
            }
            var video = request.Model.Adapt<Tastee.Infrastucture.Data.Context.Videos>();
            video.CreatedDate = Converters.DateTimeToUnixTimeStamp(DateTime.Now).Value;
            video.CreatedBy = request.CreateBy;
            var response = await _videoService.InsertAsync(video);
            if (!response.Successful)
                return response;

            if (request.Model.File != null)
            {
                List<IFormFile> files = new List<IFormFile>();
                files.Add(request.Model.File);
                var imgDict = _fileService.UploadTmpFolder(files);
                var keyPrefix = _fileService.GenerateS3KeyPrefix(video.Id, UploadFileType.Image, ObjectType.VideoImage);
                var uploadResult = await _fileService.UploadFolderToS3BucketAsync(imgDict.FolderPath, keyPrefix);
                if (!String.IsNullOrEmpty(uploadResult))
                {
                    return new UploadFilesResponse { Successful = false, Message = uploadResult };
                }
                _fileService.DeleteFolder(imgDict.FolderPath);
                var url = _fileService.GenerateAwsFileUrl(String.Format("{0}/{1}", keyPrefix, imgDict.ImgDictionary[0])).Data;
                response = await _videoService.UpdateImageAsync(video.Id, url);
                if (!response.Successful)
                {
                    response.Message = "Video Added but update image failed";
                    return response;
                }
            }
            return new Response() { Successful = true, Message ="Insert Successful" };
        }
    }
}