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
using Tastee.Application.Features.Menus.Queries;
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
        /// <param name="draw"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <param name="name"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("load-data")]
        public async Task<IActionResult> LoadData(
            [FromForm] string draw,
            [FromForm] string start,
            [FromForm] string length,
            [FromForm] string name,
            [FromForm] int? status
            )
        {
            try
            {
                int pageSize = length != null ? Convert.ToInt32(length) : Constants.DEFAULT_PAGE_SIZE;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int pageIndex = skip / pageSize + 1;
                int recordsTotal = 0;
                GetMenusQuery menusQuery = new GetMenusQuery { PageIndex = pageIndex, PageSize = pageSize, MenuName = name, Status = status };
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
        /// Create menu
        /// </summary>
        /// <param name="bannerModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(CreateMenuCommand menuModel)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage);

                return Ok(new Response { Successful = false, Message = string.Join(",", errorMessage) });
            }
            menuModel.CreatedBy = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            return Ok(await Mediator.Send(menuModel));
        }


        /// <summary>
        /// Update menu
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update(UpdateMenuCommand model)
        {
            bool isActionSuccess = false;
            try
            {
                model.UpdatedBy = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

                return Ok(await Mediator.Send(model));
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
    }
}
