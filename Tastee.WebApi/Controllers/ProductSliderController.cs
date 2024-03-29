﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Tastee.Application.Wrappers;
using Tastee.Domain.Entities;
using Tastee.Feature.ProductSliders.Queries;
using Tastee.Features.ProductSliders.Commands;
using Tastee.Features.ProductSliders.Queries;
using Tastee.Shared;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Tastee.WebApi.Controllers
{   [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductSliderController : BaseApiController
    {
        private readonly ILogger<ProductSliderController> _logger;
        public ProductSliderController(ILogger<ProductSliderController> logger)
        {
            _logger = logger;    
        }

        /// <summary>
        /// Load dữ liệu có phân trang
        /// </summary>
        /// <param name="draw"></param>
        /// <param name="start">Page index</param>
        /// <param name="length">Page Size</param>
        /// <param name="brandId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("load-data")]
        public async Task<IActionResult> LoadData(
            [FromForm] string draw,
            [FromForm] string start,
            [FromForm] string length,
            [FromForm] string brandId,
            [FromForm] DateTime? fromDate,
            [FromForm] DateTime? toDate,
            [FromForm] string status
            )
        {
            try
            {
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int pageIndex = skip / pageSize + 1;
                int recordsTotal = 0;
                GetProductSlidersQuery ProductSlidersQuery = new GetProductSlidersQuery { PageIndex = pageIndex, PageSize = pageSize, BrandId = brandId, FromDate=fromDate, ToDate = toDate, Status = status };
                var rs = await Mediator.Send(ProductSlidersQuery);

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
                    new { draw, recordsFiltered = 0, recordsTotal = 0, data = new List<ProductSlider>() });
        }

        // GET: api/ProductSliders/5
        [HttpGet("detail/{id}")]
        public async Task<IActionResult> GetProductSliderDetail(string id)
        {
            try
            {
                GetProductSliderByIdQuery rq = new GetProductSliderByIdQuery { Id = id };
                return Ok(await Mediator.Send(rq));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get ProductSlider detail, ProductSlider id: {0}", id);
            }
            finally
            {
                _logger.LogInformation("Get ProductSlider detail, ProductSlider Id {0}", id);
            }
            return Ok(new Response<ProductSlider>("Has error"));
        }

        // POST api/<ProductSliderController>
        [HttpPost]
        public async Task<IActionResult> Post(CreateProductSliderCommand ProductSliderModel)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage);

                return Ok(new Response { Successful = false, Message = string.Join(",", errorMessage) });
            }
            ProductSliderModel.CreatedBy = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            return Ok(await Mediator.Send(ProductSliderModel));
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update(UpdateProductSliderCommand model)
        {
            bool isActionSuccess = false;
            try
            {
                model.UpdateBy = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

                return Ok(await Mediator.Send(model));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update ProductSlider, ProductSlider: {0}", model);
            }
            finally
            {
                _logger.LogInformation("Update ProductSlider, ProductSlider: {0}, Result status: {1}", model, isActionSuccess);
            }
            return Ok(new { Successful = false, Error = "Has error when update ProductSlider" });
        }
    }
}