using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Tastee.Application.Features.Menus.Commands;
using Tastee.Application.Features.Menus.Items.Commands;
using Tastee.Application.Features.Menus.Items.Queries;
using Tastee.Application.Features.Menus.Queries;
using Tastee.Application.ViewModel;
using Tastee.Application.Wrappers;
using Tastee.Domain.Entities;
using Tastee.Shared;
using URF.Core.Abstractions;

namespace Tastee.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MenusController : BaseApiController
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<OperatorsController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public MenusController(
           IConfiguration configuration,
           ILogger<OperatorsController> logger,
           IUnitOfWork unitOfWork
           )
        {
            _configuration = configuration;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        #region Menus
        // GET: api/Menus/5
        /// <summary>
        /// Get menu detail
        /// </summary>
        /// <param name="id"></param>
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

        /// <summary>
        /// Load list Menu
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("load-data")]
        public async Task<IActionResult> LoadData([FromForm] GetMenusViewModel model)
        {
            try
            {
                int recordsTotal = 0;
                GetMenusQuery menusQuery = new GetMenusQuery { RequestModel = model };
                var rs = await Mediator.Send(menusQuery);

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
                    new { model.Draw, recordsFiltered = 0, recordsTotal = 0, data = new List<Menu>() });
        }

        /// <summary>
        /// Create menu
        /// </summary>
        /// <param name="menuModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(Menu menuModel)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage);

                return Ok(new Response { Successful = false, Message = string.Join(",", errorMessage) });
            }
            menuModel.CreatedBy = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            var createCommand = new CreateMenuCommand()
            {
                MenuModel = menuModel
            };
            return Ok(await Mediator.Send(createCommand));
        }


        /// <summary>
        /// Update menu
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update(Menu model)
        {
            bool isActionSuccess = false;
            try
            {
                model.UpdatedBy = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
                var updateCommand = new UpdateMenuCommand()
                {
                    MenuModel = model
                };
                return Ok(await Mediator.Send(updateCommand));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update menu, Menu: {0}", model);
            }
            finally
            {
                _logger.LogInformation("Update Menu, Menu: {0}, Result status: {1}", model, isActionSuccess);
            }
            return Ok(new { Successful = false, Error = "Has error when update menu" });
        }
        #endregion

        #region Items
        /// <summary>
        /// Get menu item detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Items/detail/{id}")]
        public async Task<IActionResult> GetMenuItemDetail(string id)
        {
            try
            {
                GetMenuItemByIdQuery rq = new GetMenuItemByIdQuery { Id = id };
                return Ok(await Mediator.Send(rq));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get MenuItem detail, id: {0}", id);
            }
            finally
            {
                _logger.LogInformation("Get MenuItem detail, Id {0}", id);
            }
            return Ok(new Response<Menu>("Has error"));
        }

        /// <summary>
        /// Load list menu items
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Items/load-data")]
        public async Task<IActionResult> ItemsLoadData([FromForm] GetMenuItemsViewModel model)
        {
            int recordsTotal = 0;
            GetMenuItemsQuery menusQuery = new GetMenuItemsQuery { RequestModel = model };
            var rs = await Mediator.Send(menusQuery);

            //total number of rows counts
            recordsTotal = rs.TotalRows;

            //Paging
            var data = rs.ListData;

            //Returning Json Data
            return new JsonResult(
                new { model.Draw, recordsFiltered = recordsTotal, recordsTotal, data });
        }

        /// <summary>
        /// Create menu item
        /// </summary>
        /// <param name="itemModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Items")]
        public async Task<IActionResult> ItemPost(MenuItem itemModel)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage);

                return Ok(new Response { Successful = false, Message = string.Join(",", errorMessage) });
            }
            itemModel.CreatedBy = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            var createCommand = new CreateMenuItemCommand()
            {
                MenuItemModel = itemModel
            };
            return Ok(await Mediator.Send(createCommand));
        }


        /// <summary>
        /// Update menu item
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Items/update")]
        public async Task<IActionResult> ItemsUpdate(MenuItem model)
        {
            bool isActionSuccess = false;
            try
            {
                model.UpdatedBy = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
                var updateCommand = new UpdateMenuItemCommand()
                {
                    MenuItemModel = model
                };
                return Ok(await Mediator.Send(updateCommand));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update MenuItem, MenuItem: {0}", model);
            }
            finally
            {
                _logger.LogInformation("Update MenuItem, MenuItem: {0}, Result status: {1}", model, isActionSuccess);
            }
            return Ok(new { Successful = false, Error = "Has error when update MenuItem" });
        }
        #endregion
    }
}
