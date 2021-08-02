using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Tastee.Application.Features.MenuItems.Commands;
using Tastee.Application.Features.MenuItems.Queries;
using Tastee.Application.Wrappers;
using Tastee.Domain.Entities;
using Tastee.Infrastucture.Data.Context;
using Tastee.Shared;

namespace Tastee.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemController : BaseApiController
    {
        private readonly ILogger<MenuItemController> _logger;

        public MenuItemController(ILogger<MenuItemController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("load-data")]
        public async Task<IActionResult> LoadData(
            [FromForm] string draw,
            [FromForm] string start,
            [FromForm] string length,
            [FromForm] string name,
            [FromForm] int status
            )
        {
            try
            {
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int pageIndex = skip / pageSize + 1;
                int recordsTotal = 0;
                GetMenuItemsQuery menuItemsQuery = new GetMenuItemsQuery { PageIndex = pageIndex, PageSize = pageSize, Name = name, Status = status};
                var rs = await Mediator.Send(menuItemsQuery);

                //total number of rows counts
                recordsTotal = rs.TotalRows;

                //Paging
                var data = rs.ListData;

                //Returning Json Data
                return new JsonResult(
                    new { draw, recordsFiltered = recordsTotal, recordsTotal, data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LoadData");
            }

            return new JsonResult(
                    new { draw, recordsFiltered = 0, recordsTotal = 0, data = new List<MenuItem>() });
        }

        /// <summary>
        /// Thêm mới MenuItem
        /// </summary>
        /// <param name="createMenuItemCommand"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(CreateMenuItemCommand createMenuItemCommand)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    createMenuItemCommand.CreatedBy = CurrentUser;
                    return Ok(await Mediator.Send(createMenuItemCommand));
                }
                else
                {
                    var errorMessage = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage);
                    return Ok(new Response { Successful = false, Message = string.Join(",", errorMessage) });
                }
            }
            catch (Exception ex)
            {
                
                _logger.LogError(ex, "Insert menuItem, MenuItem: {0}", ObjectToJson(createMenuItemCommand));
            }
            finally
            {
                _logger.LogInformation("Insert menuItem, MenuItem: {0}", ObjectToJson(createMenuItemCommand));
            }
            return Ok(new { Successful = false, Error = "Has error when insert menuItem" });
        }

        /// <summary>
        /// Cập nhật menuItem
        /// </summary>
        /// <param name="updateMenuItemCommand"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update(UpdateMenuItemCommand updateMenuItemCommand)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    updateMenuItemCommand.UpdatedBy = CurrentUser;
                    return Ok(await Mediator.Send(updateMenuItemCommand));
                }
                else
                {
                    var errorMessage = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage);
                    return Ok(new Response { Successful = false, Message = string.Join(",", errorMessage) });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update MenuItem, MenuItem: {0}", ObjectToJson(updateMenuItemCommand));
            }
            finally
            {
                _logger.LogInformation("Update MenuItem, MenuItem: {0}", ObjectToJson(updateMenuItemCommand));
            }
            return Ok(new { Successful = false, Error = "Has error when update MenuItem" });
        }

        /// <summary>
        /// Lấy thông tin chi tiết Menu Item
        /// </summary>
        /// <param name="id">Id của Menu Item</param>
        /// <returns></returns>
        [HttpGet("detail/{id}")]
        public async Task<IActionResult> GetMenuItemDetail(string id)
        {
            try
            {
                GetMenuItemByIdQuery rq = new GetMenuItemByIdQuery { Id = id };
                return Ok(await Mediator.Send(rq));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get Menu Item detail, Menu Item id: {0}", id);
            }
            finally
            {
                _logger.LogInformation("Get Menu Item detail, Menu Item Id {0}", id);
            }
            return Ok(new Response<MenuItem>("Has error"));
        }

    }
}