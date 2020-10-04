using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tastee.Application.Wrappers;
using Tastee.Domain.Entities;
using Tastee.Feature.Brands.Queries;
using Tastee.Features.Brands.Commands;
using Tastee.Features.Brands.Queries;
using Tastee.Infrastucture.Data.Context;
using Tastee.Shared;

namespace Tastee.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : BaseApiController
    {
        private readonly ILogger<BrandsController> _logger;

        public BrandsController(
            ILogger<BrandsController> logger
            )
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("load-data")]
        [AllowAnonymous]
        public async Task<IActionResult> LoadData(
            [FromForm] string draw,
            [FromForm] string start,
            [FromForm] string length
            , [FromForm] string name
            )
        {
            try
            {
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int pageIndex = skip / pageSize + 1;
                int recordsTotal = 0;
                GetBrandsQuery brandsQuery = new GetBrandsQuery { PageIndex = pageIndex, PageSize = pageSize, BrandName = name };
                var rs = await Mediator.Send(brandsQuery);

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
                    new { draw, recordsFiltered = 0, recordsTotal = 0, data = new List<Users>() });
        }

        // GET: api/Brands/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                GetBrandByIdQuery rq = new GetBrandByIdQuery { Id = id };
                return Ok(await Mediator.Send(rq));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get brand detail, brand id: {0}", id);
            }
            finally
            {
                _logger.LogInformation("Get brand detail, brand Id {0}", id);
            }
            return Ok(new Response<Brand>("Has error"));
        }

        // POST: api/Brands
        [HttpPost]
        public async Task<IActionResult> Post(CreateBrandCommand brandModel)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage);

                return Ok(new Response { Successful = false, Message = string.Join(",", errorMessage) });
            }

            return Ok(await Mediator.Send(brandModel));
        }

        // PUT: api/Brands/5
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update(UpdateBrandCommand model)
        {
            bool isActionSuccess = false;
            try
            {
                return Ok(await Mediator.Send(model));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update brand, Brand: {0}", model);
            }
            finally
            {
                _logger.LogInformation("Update Brand, Brand: {0}, Result status: {1}", model, isActionSuccess);
            }
            return Ok(new { Successful = false, Error = "Has error when update" });
        }
    }
}