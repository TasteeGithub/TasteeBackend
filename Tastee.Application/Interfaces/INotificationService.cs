using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tastee.Infrastucture.Data.Context;
using Tastee.Shared;
using Tastee.Shared.Models.Notifications;

namespace Tastee.Application.Interfaces
{
    public interface INotificationService : ITasteeServices<Notifications>
    {
        Task<PaggingModel<NotificationModel>> GetNotificationAsync(GetNotifycationViewModel requestModel);
    }
}
