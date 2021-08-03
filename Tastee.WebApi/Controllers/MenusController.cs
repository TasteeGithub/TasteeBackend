using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tastee.Application.Features.Menus.Queries;
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
    }
}
