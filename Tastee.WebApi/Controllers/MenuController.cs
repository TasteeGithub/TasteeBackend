using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Tastee.Application.Features.Menus.Commands;
using Tastee.Application.Features.Menus.Queries;
using Tastee.Application.Wrappers;
using Tastee.Domain.Entities;
using Tastee.Infrastucture.Data.Context;
using Tastee.Shared;

namespace Tastee.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : BaseApiController
    {
        private readonly ILogger<MenuController> _logger;

        public MenuController(ILogger<MenuController> logger)
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
                GetMenusQuery menusQuery = new GetMenusQuery { PageIndex = pageIndex, PageSize = pageSize, Name = name, Status = status};
                var rs = await Mediator.Send(menusQuery);

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
                    new { draw, recordsFiltered = 0, recordsTotal = 0, data = new List<Menu>() });
        }

        /// <summary>
        /// Thêm mới Menu
        /// </summary>
        /// <param name="createMenuCommand"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(CreateMenuCommand createMenuCommand)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    createMenuCommand.CreatedBy = CurrentUser;
                    return Ok(await Mediator.Send(createMenuCommand));
                }
                else
                {
                    var errorMessage = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage);
                    return Ok(new Response { Successful = false, Message = string.Join(",", errorMessage) });
                }
            }
            catch (Exception ex)
            {
                
                _logger.LogError(ex, "Insert menu, Menu: {0}", ObjectToJson(createMenuCommand));
            }
            finally
            {
                _logger.LogInformation("Insert menu, Menu: {0}", ObjectToJson(createMenuCommand));
            }
            return Ok(new { Successful = false, Error = "Has error when insert menu" });
        }

        /// <summary>
        /// Cập nhật menu
        /// </summary>
        /// <param name="updateMenuCommand"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update(UpdateMenuCommand updateMenuCommand)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    updateMenuCommand.UpdatedBy = CurrentUser;
                    return Ok(await Mediator.Send(updateMenuCommand));
                }
                else
                {
                    var errorMessage = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage);
                    return Ok(new Response { Successful = false, Message = string.Join(",", errorMessage) });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update Menu, Menu: {0}", ObjectToJson(updateMenuCommand));
            }
            finally
            {
                _logger.LogInformation("Update Menu, Menu: {0}", ObjectToJson(updateMenuCommand));
            }
            return Ok(new { Successful = false, Error = "Has error when update Menu" });
        }

        /// <summary>
        /// Lấy thông tin chi tiết menu
        /// </summary>
        /// <param name="id">Id của menu</param>
        /// <returns></returns>
        [HttpGet("detail/{id}")]
        public async Task<IActionResult> GetMenuDetail(string id)
        {
            try
            {
                GetMenuByIdQuery rq = new GetMenuByIdQuery { Id = id };
                return Ok(await Mediator.Send(rq));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get menu detail, menu id: {0}", id);
            }
            finally
            {
                _logger.LogInformation("Get menu detail, menu Id {0}", id);
            }
            return Ok(new Response<Menu>("Has error"));
        }

    }
}