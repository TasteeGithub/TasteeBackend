using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Application.ViewModel;
using Tastee.Domain.Entities;
using Tastee.Shared;
using Tastee.Shared.Models.Notifications;

namespace Tastee.Application.Features.Notifications.Queries
{
    public class GetNotificationsQuery : IRequest<PaggingModel<NotificationModel>>
    {
        public GetNotifycationViewModel RequestModel;

        public class GetNotificationsQueryHandler : IRequestHandler<GetNotificationsQuery, PaggingModel<NotificationModel>>
        {
            private readonly INotificationService _notificationsService;
            public GetNotificationsQueryHandler(INotificationService notificationsService)
            {
                _notificationsService = notificationsService;
            }

            public async Task<PaggingModel<NotificationModel>> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
            {
                return await _notificationsService.GetNotificationAsync(request.RequestModel);
            }
        }
    }
}
