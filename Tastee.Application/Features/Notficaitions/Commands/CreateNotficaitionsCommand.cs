using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Application.Utilities;
using Tastee.Application.Wrappers;
using Tastee.Shared;
using Tastee.Shared.Models.Files;
using Tastee.Shared.Models.Notifications;

namespace Tastee.Application.Features.Notficaitions.Commands
{
    public class NotificationTaskData
    {
        public Tastee.Infrastucture.Data.Context.Notifications notification { get; set; }
        public List<string> sendToIds { get; set; }
    }

    public class CreateNotificationCommand : IRequest<Response>
    {
        public InsertNotificationViewModel Model;
        public string CreateBy;
    }

    public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, Response>
    {
        private readonly INotificationService _notificationService;
        private readonly IFileService _fileService;
        private readonly IBrandService _brandService;
        private readonly IGenericService<Infrastucture.Data.Context.Users> _userService;

        public CreateNotificationCommandHandler(INotificationService notificationService, IFileService fileService, IBrandService brandService, IGenericService<Infrastucture.Data.Context.Users> userService)
        {
            _notificationService = notificationService;
            _fileService = fileService;
            _brandService = brandService;
            _userService = userService;
        }

        public async Task<Response> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            if (request.Model.File != null && !_fileService.IsValidFile(request.Model.File, UploadFileType.Image))
            {
                return new UploadFilesResponse { Successful = false, Message = "Invalid file" };
            }
            var notification = request.Model.Adapt<Tastee.Infrastucture.Data.Context.Notifications>();
            notification.CreatedDate = Converters.DateTimeToUnixTimeStamp(DateTime.Now).Value;
            notification.CreatedBy = request.CreateBy;
            notification.Id = Guid.NewGuid().ToString();
            if (request.Model.SendToIds.Count == 0)
            {
                notification.SendAll = true;
            }

            if (request.Model.File != null)
            {
                List<IFormFile> files = new List<IFormFile>();
                files.Add(request.Model.File);
                var imgDict = _fileService.UploadTmpFolder(files);
                var keyPrefix = _fileService.GenerateS3KeyPrefix(notification.Id, UploadFileType.Image, ObjectType.Notification);
                var uploadResult = await _fileService.UploadFolderToS3BucketAsync(imgDict.FolderPath, keyPrefix);
                if (!String.IsNullOrEmpty(uploadResult))
                {
                    return new UploadFilesResponse { Successful = false, Message = uploadResult };
                }
                _fileService.DeleteFolder(imgDict.FolderPath);
                var url = _fileService.GenerateAwsFileUrl(String.Format("{0}/{1}", keyPrefix, imgDict.ImgDictionary[0])).Data;
                notification.Image = url;
            }
            var response = await _notificationService.InsertAsync(notification);
            if (!response.Successful)
                return response;

            ThreadPool.QueueUserWorkItem(BackgroundNotificationTaskWithObject, new NotificationTaskData { notification = notification, sendToIds = request.Model.SendToIds ?? new List<string>() });

            return new Response() { Successful = true, Message = "Insert Successful" };
        }

        private void BackgroundNotificationTaskWithObject(Object stateInfo)
        {
            NotificationTaskData data = (NotificationTaskData)stateInfo;
            var userIds = CreateNotificationMappings(data.notification, data.sendToIds);
            if (userIds.Count > 0)
            {
                SendFCM(data.notification, userIds);
            }
        }


        private List<string> CreateNotificationMappings(Tastee.Infrastucture.Data.Context.Notifications notification, List<string> sendToIds)
        {
            List<Tastee.Infrastucture.Data.Context.NotificationMapping> mappings = new List<Infrastucture.Data.Context.NotificationMapping>();
            if (notification.Type == (int)NotificationType.Brand)
            {
                List<Tastee.Infrastucture.Data.Context.Brands> brands = new List<Tastee.Infrastucture.Data.Context.Brands>();

                if (notification.SendAll)
                {
                    brands = _brandService.GetAllActiveBrands();
                }
                else
                {
                    brands = _brandService.GeBrandsByIds(sendToIds);
                }

                if (brands.Count > 0)
                {
                    var brandIds = brands.Select(x => x.Id).ToList();
                    List<Tastee.Infrastucture.Data.Context.BrandMerchants> merchants = _brandService.GetByBrandIds(brandIds);
                    foreach (var merchant in merchants)
                    {
                        mappings.Add(new Infrastucture.Data.Context.NotificationMapping()
                        {
                            BrandId = merchant.BrandId,
                            NotificationId = notification.Id,
                            UserId = merchant.UserId,
                        });
                    }

                }

            }
            else
            {
                List<string> userIds = new List<string>();
                if (notification.SendAll)
                {
                    userIds = _userService.Queryable().Where(x => x.Status == AccountStatus.Active.ToString()).Select(x => x.Id).ToList();
                }
                else
                {
                    userIds = _userService.Queryable().Where(x => sendToIds.Contains(x.Id) && x.Status == AccountStatus.Active.ToString()).Select(x => x.Id).ToList();
                }
                if (userIds.Count > 0)
                {
                    foreach (var userId in userIds)
                    {
                        mappings.Add(new Infrastucture.Data.Context.NotificationMapping()
                        {
                            NotificationId = notification.Id,
                            UserId = userId,
                        });
                    }
                }
            }

            if (mappings.Count > 0)
            {
                _notificationService.InsertMappingAsync(mappings);
            }
            return mappings.Select(x => x.UserId).ToList();
        }

        private void SendFCM(Infrastucture.Data.Context.Notifications notificiation, List<string> userIds)
        {
            _notificationService.SendNotification(notificiation, userIds);
        }


    }
}
