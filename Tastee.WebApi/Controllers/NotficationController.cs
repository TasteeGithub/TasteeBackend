using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Tastee.Application.Features.Notficaitions.Commands;
using Tastee.Application.Features.Notifications.Queries;
using Tastee.Application.Wrappers;
using Tastee.Shared;
using Tastee.Shared.Models.Notifications;

namespace Tastee.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NotficationController : BaseApiController
    {
        private readonly ILogger<ToppingController> _logger;
        public NotficationController(ILogger<ToppingController> logger)
        {
            _logger = logger;
        }


        [HttpPost]
        [Route("load-data")]
        public async Task<IActionResult> LoadData([FromForm] GetNotifycationViewModel model)
        {
            try
            {
                int recordsTotal = 0;
                GetNotificationsQuery notificationQuery = new GetNotificationsQuery { RequestModel = model };
                var rs = await Mediator.Send(notificationQuery);

                //total number of rows counts
                recordsTotal = rs.TotalRows;

                //Paging
                var data = rs.ListData;

                //Returning Json Data
                return new JsonResult(
                    new { model.Draw, recordsFiltered = recordsTotal, recordsTotal, data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LoadData");
            }

            return new JsonResult(
                    new { model.Draw, recordsFiltered = 0, recordsTotal = 0, data = new List<NotificationModel>() });
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] InsertNotificationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage);

                return Ok(new Response { Successful = false, Message = string.Join(",", errorMessage) });
            }
            var createCommand = new CreateNotificationCommand()
            {
                Model = model,
                CreateBy = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value
            };

            return Ok(await Mediator.Send(createCommand));
        }
    }
}
