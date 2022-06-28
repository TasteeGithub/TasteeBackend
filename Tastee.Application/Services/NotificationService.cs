using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using LinqKit;
using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Application.Utilities;
using Tastee.Infrastucture.Data.Context;
using Tastee.Shared;
using Tastee.Shared.Models.Categories;
using Tastee.Shared.Models.Notifications;
using Tastee.Shared.Models.Notifications.Firebase;
using URF.Core.Abstractions;

namespace Tastee.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IGenericService<Notifications> _serviceNotifications;
        private readonly IGenericService<NotificationMapping> _serviceNotificationMapping;
        private readonly IGenericService<DeviceTokens> _serviceDeviceTokens;


        public NotificationService(
          ILogger<NotificationService> logger,
          IUnitOfWork unitOfWork,
          IGenericService<Notifications> serviceNotifications,
          IGenericService<NotificationMapping> serviceNotificationMapping,
          IConfiguration configuration,
          IGenericService<DeviceTokens> serviceDeviceTokens
          )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _serviceNotifications = serviceNotifications;
            _serviceNotificationMapping = serviceNotificationMapping;
            _configuration = configuration;
            _serviceDeviceTokens = serviceDeviceTokens;
        }

        public Task<Notifications> GetByIdAsync(string id)
        {
            return _serviceNotifications.FindAsync(id);
        }

        public async Task<Response> InsertAsync(Notifications model)
        {
            _serviceNotifications.Insert(model);
            await _unitOfWork.SaveChangesAsync();
            return new Response { Successful = true, Message = "Add notification successed" };
        }

        public Task<Response> UpdateAsync(Notifications model)
        {
            throw new NotImplementedException();
        }

        public async Task<PaggingModel<NotificationModel>> GetNotificationAsync(GetNotifycationViewModel requestModel)
        {
            ExpressionStarter<Notifications> searchCondition = PredicateBuilder.New<Notifications>(true);
            int pageSize = Converters.StringToInteger(requestModel.Length, Constants.DEFAULT_PAGE_SIZE).Value;
            int skip = Converters.StringToInteger(requestModel.Start).Value;
            int pageIndex = skip / pageSize + 1;

            if (requestModel.Type != null)
            {
                searchCondition = searchCondition.And(x => x.Type == (int)requestModel.Type);
            }

            var listNotifications = _serviceNotifications.Queryable().Where(searchCondition).OrderByDescending(x => x.CreatedDate);

            var pagedListNotifications = await PaginatedList<Notifications>.CreateAsync(listNotifications, pageIndex, pageSize);
            var notificationIds = pagedListNotifications.Where(x => x.SendAll == false).Select(x => x.Id).ToList();
            var notificationMappings = _serviceNotificationMapping.Queryable().Where(x => notificationIds.Contains(x.NotificationId)).ToList();

            PaggingModel<NotificationModel> returnResult = new PaggingModel<NotificationModel>()
            {
                ListData = pagedListNotifications.Select(x => BuildNotificationModel(x, notificationMappings)).ToList(),
                TotalRows = pagedListNotifications.TotalRows,
            };

            return returnResult;
        }

        private NotificationModel BuildNotificationModel(Notifications notification, List<NotificationMapping> listMapping)
        {
            NotificationModel model = notification.Adapt<NotificationModel>();
            if (!model.SendAll)
                if (model.Type == (int)NotificationType.Brand)
                    model.SendToIds = listMapping.Where(x => x.NotificationId == notification.Id).Select(x => x.BrandId).ToList();
                else
                    model.SendToIds = listMapping.Where(x => x.NotificationId == notification.Id).Select(x => x.UserId).ToList();
            return model;
        }

        public async Task<Response> InsertMappingAsync(List<NotificationMapping> mappins)
        {
            foreach (var mapping in mappins)
                _serviceNotificationMapping.Insert(mapping);
            await _unitOfWork.SaveChangesAsync();
            return new Response { Successful = true, Message = "Add notification mapping successed" };
        }


        public async Task<Response> DeleteCategoryAsync(string id)
        {
            var notfication = await GetByIdAsync(id);
            if (notfication == null)
            {
                return new Response { Successful = true, Message = "Delete notfication successed" };
            }
            _serviceNotifications.Delete(notfication);


            var mappings = _serviceNotificationMapping.Queryable().Where(x => x.NotificationId == notfication.Id).ToList();
            foreach (var mapping in mappings)
                _serviceNotificationMapping.Delete(mapping);
            await _unitOfWork.SaveChangesAsync();
            return new Response { Successful = true, Message = "Delete notfication successed" };
        }
        #region Firebase
        public async Task<int> SendNotification(Notifications notification, List<string> UserIds)
        {
            try
            {
                FirebaseConfig fbconfig = new FirebaseConfig();
                _configuration.Bind("Firebase", fbconfig);

                var json = JsonConvert.SerializeObject(fbconfig);
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromJson(json),
                });

                var registrationTokens = _serviceDeviceTokens.Queryable().Where(x => UserIds.Contains(x.UserId) && x.AllowPush == true).Select(x => x.DeviceToken).ToList();
                if (registrationTokens.Count > 0)
                {
                    var message = new MulticastMessage()
                    {
                        Notification = new Notification
                        {
                            Title = notification.Title,
                            Body = notification.Description,
                            ImageUrl = notification.Image ?? ""
                        },
                        Tokens = registrationTokens,
                        Data = new Dictionary<string, string>() { { "title", notification.Title }, { "body", notification.Description }, },
                    };
                    var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message).ConfigureAwait(true);
                    return response.SuccessCount;
                }
                return 0;
            }
            catch (Exception ex)
            {
                _logger.LogError("send FCM failed", ex);
                return 0;
            }
        }
        #endregion
    }
}
