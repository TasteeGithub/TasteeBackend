using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Tastee.Application.Features.Brands.Commands;
using Tastee.Application.ViewModel;
using Tastee.Application.Wrappers;
using Tastee.Domain.Entities;
using Tastee.Domain.Models.Brands;
using Tastee.Feature.Brands.Queries;
using Tastee.Features.Brands.Commands;
using Tastee.Features.Brands.Queries;
using Tastee.Infrastucture.Data.Context;
using Tastee.Shared;

namespace Tastee.WebApi.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> LoadData([FromForm] GetBrandsViewModel model)
        {
            try
            {
                int recordsTotal = 0;
                GetBrandsQuery brandsQuery = new GetBrandsQuery { RequestModel = model };
                var rs = await Mediator.Send(brandsQuery);

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
                    new { model.Draw, recordsFiltered = 0, recordsTotal = 0, data = new List<Brand>() });
        }

        // GET: api/Brands/5
        [HttpGet("detail/{id}")]
        public async Task<IActionResult> GetBrandDetail(string id)
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
        public async Task<IActionResult> Post(Brand model)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage);

                return Ok(new Response { Successful = false, Message = string.Join(",", errorMessage) });
            }
            model.UpdateBy = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            var createCommand = new CreateBrandCommand()
            {
                BrandModel = model
            };

            return Ok(await Mediator.Send(createCommand));
        }

        /// <summary>
        /// Update brand
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update(Brand model)
        {
            bool isActionSuccess = false;
            try
            {
                model.UpdateBy = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
                var updateCommand = new UpdateBrandCommand()
                {
                    BrandModel = model
                };
                return Ok(await Mediator.Send(updateCommand));
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


        [HttpPost]
        [Route("uploadimages")]
        public async Task<IActionResult> UploadImages([FromForm] UploadBrandImageDto request)
        {
            bool isActionSuccess = false;
            try
            {
                var uploadCommand = new UploadBrandImagesCommand()
                {
                    RequestModel = request,
                    UploadBy = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value
                };

                var result = await Mediator.Send(uploadCommand);
                if (result.Successful)
                    isActionSuccess = true;
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Upload Brand Images, Brand: {0}", request.BrandID);
            }
            finally
            {
                _logger.LogInformation("Upload Brand, Brand: {0}, Result status: {1}", request.BrandID, isActionSuccess);
            }
            return Ok(new { Successful = false, Error = "Has error when upload" });
        }
    }
}