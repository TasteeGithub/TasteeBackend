using FirebaseAdmin.Messaging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tastee.Infrastucture.Data.Context;
using Tastee.Shared;
using Tastee.Shared.Models.Notifications;
using Tastee.Shared.Models.Notifications.Firebase;

namespace Tastee.Application.Interfaces
{
    public interface INotificationService : ITasteeServices<Notifications>
    {
        Task<PaggingModel<NotificationModel>> GetNotificationAsync(GetNotifycationViewModel requestModel);
        Task<Response> InsertMappingAsync(List<NotificationMapping> mappings);

        #region Firebase
        Task<int> SendNotification(Notifications notification, List<string> UserIds);
        Task<Response> DeleteCategoryAsync(string id);
        #endregion
    }
}
