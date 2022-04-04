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
using Tastee.Shared.Models.Topping;
using Tastee.Features.Toppings.Queries;
using Tastee.Features.Toppings.Commands;

namespace Tastee.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ToppingController : BaseApiController
    {
        private readonly ILogger<ToppingController> _logger;
        public ToppingController(ILogger<ToppingController> logger)
        {
            _logger = logger;
        }

        #region Topping
        [HttpPost]
        [Route("load-data")]
        public async Task<IActionResult> LoadData([FromForm] GetToppingViewModel model)
        {
            try
            {
                int recordsTotal = 0;
                GetToppingsQuery groupQuery = new GetToppingsQuery { RequestModel = model };
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
                    new { model.Draw, recordsFiltered = 0, recordsTotal = 0, data = new List<Topping>() });
        }


        [HttpGet("detail/{id}")]
        public async Task<IActionResult> GetToppingsDetail(string id)
        {
            try
            {
                GetToppingByIdQuery rq = new GetToppingByIdQuery { Id = id };
                return Ok(await Mediator.Send(rq));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get topping detail, id: {0}", id);
            }
            finally
            {
                _logger.LogInformation("Get topping detail, Id {0}", id);
            }
            return Ok(new Response<Topping>("Has error"));
        }


        [HttpPost]
        public async Task<IActionResult> Post(Topping model)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage);

                return Ok(new Response { Successful = false, Message = string.Join(",", errorMessage) });
            }
            model.CreatedBy = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            var createCommand = new CreateToppingsCommand()
            {
                ToppingsModel = model
            };
            return Ok(await Mediator.Send(createCommand));
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update(UpdateToppingViewModel model)
        {
            try
            {
                var updateCommand = new UpdateToppingsCommand()
                {
                    Model = model,
                    UserEmail = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value
                };
                return Ok(await Mediator.Send(updateCommand));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update topping, topping: {0}", model);
            }
            finally
            {
                _logger.LogInformation("Update topping, topping: {0}", model);
            }
            return Ok(new { Successful = false, Error = "Has error when update" });
        }

        [HttpPost]
        [Route("delete")]
        public async Task<IActionResult> Delete(string Id)
        {
            try
            {
                var deleteCommand = new DeleteToppingsCommand()
                {
                    Id = Id
                };
                return Ok(await Mediator.Send(deleteCommand));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete topping, Id: {0}", Id);
            }
            finally
            {
                _logger.LogInformation("Update topping, Id: {0}", Id);
            }
            return Ok(new { Successful = false, Error = "Has error when delete" });
        }
        #endregion

        #region GroupTopping
        [HttpPost]
        [Route("group/load-data")]
        public async Task<IActionResult> GroupToppings_LoadData([FromForm] GetGroupToppingViewModel model)
        {
            try
            {
                int recordsTotal = 0;
                GetGroupToppingsQuery groupQuery = new GetGroupToppingsQuery { RequestModel = model };
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
                    new { model.Draw, recordsFiltered = 0, recordsTotal = 0, data = new List<GroupTopping>() });
        }

        
        [HttpGet("group/detail/{id}")]
        public async Task<IActionResult> GetGroupToppingsDetail(string id)
        {
            try
            {
                GetGroupToppingByIdQuery rq = new GetGroupToppingByIdQuery { Id = id };
                return Ok(await Mediator.Send(rq));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get group toppings detail, id: {0}", id);
            }
            finally
            {
                _logger.LogInformation("Get group toppings detail, Id {0}", id);
            }
            return Ok(new Response<GroupTopping>("Has error"));
        }

        
        [HttpPost]
        [Route("group")]
        public async Task<IActionResult> GroupToppings_Post(GroupTopping model)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage);

                return Ok(new Response { Successful = false, Message = string.Join(",", errorMessage) });
            }
            model.CreatedBy = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            var createCommand = new CreateGroupToppingsCommand()
            {
                GroupToppingsModel = model
            };
            return Ok(await Mediator.Send(createCommand));
        }

        [HttpPost]
        [Route("group/update")]
        public async Task<IActionResult> GroupToppings_Update(UpdateGroupToppingViewModel model)
        {
            try
            {
                var updateCommand = new UpdateGroupToppingsCommand()
                {
                    Model = model,
                    UserEmail = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value
                };
                return Ok(await Mediator.Send(updateCommand));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update group topping, group topping: {0}", model);
            }
            finally
            {
                _logger.LogInformation("Update group topping, group topping: {0}", model);
            }
            return Ok(new { Successful = false, Error = "Has error when update" });
        }

        [HttpPost]
        [Route("group/delete")]
        public async Task<IActionResult> GroupToppings_Delete(string Id)
        {
            try
            {
                var deleteCommand = new DeleteGroupToppingsCommand()
                {
                   Id = Id
                };
                return Ok(await Mediator.Send(deleteCommand));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete group topping, Id: {0}", Id);
            }
            finally
            {
                _logger.LogInformation("Update group topping, Id: {0}", Id);
            }
            return Ok(new { Successful = false, Error = "Has error when delete" });
        }
        #endregion
    }
}
