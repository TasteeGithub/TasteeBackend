using LinqKit;
using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Application.Utilities;
using Tastee.Infrastucture.Data.Context;
using Tastee.Shared;
using Tastee.Shared.Models.Categories;
using Tastee.Shared.Models.Notifications;
using URF.Core.Abstractions;

namespace Tastee.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        private readonly IGenericService<Notifications> _serviceNotifications;
        private readonly IGenericService<NotificationMapping> _serviceNotificationMapping;


        public NotificationService(
          ILogger<NotificationService> logger,
          IUnitOfWork unitOfWork,
          IGenericService<Notifications> serviceNotifications,
          IGenericService<NotificationMapping> serviceNotificationMapping)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _serviceNotifications = serviceNotifications;
            _serviceNotificationMapping = serviceNotificationMapping;
        }

        public Task<Notifications> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
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
                    model.SendToIds = listMapping.Select(x => x.BrandId).ToList();
                else
                    model.SendToIds = listMapping.Select(x => x.UserId).ToList();
            return model;
        }
    }
}
