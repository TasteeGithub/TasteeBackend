
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tastee.Application.Wrappers;
using Tastee.Domain.Entities;
using Tastee.Features.GroupItems.Queries;
using Tastee.Shared.Models.GroupItems;
using Tastee.Shared;
using System.Security.Claims;
using Tastee.Features.GroupItems.Commands;

namespace Tastee.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GroupItemsController : BaseApiController
    {

        private readonly ILogger<GroupItemsController> _logger;
        public GroupItemsController(ILogger<GroupItemsController> logger)
        {
            _logger = logger;
        }

        #region GroupItems
        [HttpPost]
        [Route("load-data")]
        public async Task<IActionResult> LoadData([FromForm] GetGroupItemViewModel model)
        {
            try
            {
                int recordsTotal = 0;
                GetGroupItemsQuery groupQuery = new GetGroupItemsQuery { RequestModel = model };
                var rs = await Mediator.Send(groupQuery);

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
                    new { model.Draw, recordsFiltered = 0, recordsTotal = 0, data = new List<GroupItem>() });
        }

        // GET: api/GroupItem/5
        [HttpGet("detail/{id}")]
        public async Task<IActionResult> GetGroupItemDetail(string id)
        {
            try
            {
                GetGroupItemsByIdQuery rq = new GetGroupItemsByIdQuery { Id = id };
                return Ok(await Mediator.Send(rq));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get group item detail, id: {0}", id);
            }
            finally
            {
                _logger.LogInformation("Get group item detail, Id {0}", id);
            }
            return Ok(new Response<GroupItem>("Has error"));
        }

        // POST: api/GroupItems
        [HttpPost]
        public async Task<IActionResult> Post(GroupItem model)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage);

                return Ok(new Response { Successful = false, Message = string.Join(",", errorMessage) });
            }
            model.CreatedBy = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            var createCommand = new CreateGroupItemsCommand()
            {
                GroupItemModel = model
            };

            return Ok(await Mediator.Send(createCommand));
        }

        #endregion

        #region GroupItemsMapping
        // POST: api/GroupItems/Items
        [Route("items")]
        [HttpPost]
        public async Task<IActionResult> GroupItemsMapping_Post(InsertGroupItemMappingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage);

                return Ok(new Response { Successful = false, Message = string.Join(",", errorMessage) });
            }
            var createCommand = new CreateGroupItemsMappingCommand()
            {
                Model = model,
                UserEmail = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value
            };
            return Ok(await Mediator.Send(createCommand));
        }
        #endregion
    }
}
