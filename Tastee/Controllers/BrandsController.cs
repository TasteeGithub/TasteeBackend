using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tastee.Shared;
using Tastee.Application.Interfaces;
using Tastee.Infrastucture.Data.Context;
using Tastee.Domain.Entities;

using MediatR;
using System.Threading;
using Tastee.Application.Wrappers;
using Tastee.Feature.Brands.Queries;
using Tastee.Features.Brands.Queries;

namespace Tastee.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : BaseApiController//ControllerBase
    {
        private readonly IBrandService  _brandService;
        private readonly ILogger<BrandsController> _logger;
        public BrandsController(
            ILogger<BrandsController> logger,
            IBrandService brandService // TODO: Sau khi chuyen he sang dung CQRS thi bo di
            )
        {
            _logger = logger;
            _brandService = brandService;
        }

        [HttpPost]
        [Route("load-data")]
        [AllowAnonymous]
        public async Task<IActionResult> LoadData(
            [FromForm] string draw,
            [FromForm] string start,
            [FromForm] string length
            ,[FromForm] string name
            )
        {
            try
            {
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int pageIndex = skip / pageSize + 1;
                int recordsTotal = 0;
                GetBrandsQuery brandsQuery = new GetBrandsQuery { PageIndex = pageIndex, PageSize = pageSize, BrandName = name };
                var rs = await this.Mediator.Send(brandsQuery);

                //var rs = await _brandService.GetBrandsAsync(pageSize, pageIndex, name);

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
        //// GET: api/Brands
        //[HttpGet]
        //public async Task<PaggingModel<Brands>> Get(int pageSize, int? pageIndex, string name)
        //{
        //    ExpressionStarter<Brands> searchCondition = PredicateBuilder.New<Brands>(true);

        //    if (name != null && name.Length > 0)
        //    {
        //        searchCondition = searchCondition.And(x => x.Name.ToLower().Contains(name.ToLower()));
        //    }

        //    var listBrands = _serviceBrands.Queryable().Where(searchCondition).OrderByDescending(x => x.CreatedDate);

        //    var pagedListUser = await PaginatedList<Brands>.CreateAsync(listBrands, pageIndex ?? 1, pageSize);

        //    PaggingModel<Brands> returnResult = new PaggingModel<Brands>()
        //    {
        //        ListData = pagedListUser.Adapt<List<Brands>>(),
        //        TotalRows = pagedListUser.TotalRows,
        //    };

        //    return returnResult;
        //}

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
            return Ok( new Response<Brand>("Has error"));

        }

        // POST: api/Brands
        [HttpPost]
        public async Task<IActionResult> Post(Brand brandModel)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage);
                
                return Ok(new Response { Successful = false, Message = string.Join(",", errorMessage) });
            }

            return Ok(await _brandService.InsertAsync(brandModel));

        }

        // PUT: api/Brands/5
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update(Brand model)
        {
            bool isActionSuccess = false;
            try
            {
                var result = await _brandService.UpdateAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update user, User: {0}", model);
            }
            finally
            {
                _logger.LogInformation("Update user, User: {0}, Result status: {1}", model, isActionSuccess);
            }
            return Ok(new { Successful = false, Error = "Has error when update" });
        }
    }
}
