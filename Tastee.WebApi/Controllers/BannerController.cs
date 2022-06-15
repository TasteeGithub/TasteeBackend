using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Tastee.Application.Features.Banners.Commands;
using Tastee.Application.Wrappers;
using Tastee.Domain.Entities;
using Tastee.Feature.Banners.Queries;
using Tastee.Features.Banners.Commands;
using Tastee.Features.Banners.Queries;
using Tastee.Shared;
using Tastee.Shared.Models.Banners;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Tastee.WebApi.Controllers
{   [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BannerController : BaseApiController
    {
        private readonly ILogger<BannerController> _logger;
        public BannerController(ILogger<BannerController> logger)
        {
            _logger = logger;    
        }

        /// <summary>
        /// Load dữ liệu có phân trang cho table .net
        /// </summary>
        /// <param name="draw"></param>
        /// <param name="start">Page index</param>
        /// <param name="length">Page Size</param>
        /// <param name="name"></param>
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
            [FromForm] string name,
            [FromForm] long? fromDate,
            [FromForm] long? toDate,
            [FromForm] string status,
            [FromForm] int? type,
            [FromForm] bool? isDisplay
            )
        {
            try
            {
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int pageIndex = skip / pageSize + 1;
                int recordsTotal = 0;
                GetBannersQuery bannersQuery = new GetBannersQuery { 
                    PageIndex = pageIndex, 
                    PageSize = pageSize, 
                    BannerName = name, 
                    FromDate=fromDate, 
                    ToDate = toDate, 
                    Status = status,
                    Type = type,
                    IsDisplay = isDisplay
                };
                var rs = await Mediator.Send(bannersQuery);

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
                    new { draw, recordsFiltered = 0, recordsTotal = 0, data = new List<Banner>() });
        }

        // GET: api/Banners/5
        [HttpGet("detail/{id}")]
        public async Task<IActionResult> GetBannerDetail(string id)
        {
            try
            {
                GetBannerByIdQuery rq = new GetBannerByIdQuery { Id = id };
                return Ok(await Mediator.Send(rq));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get banner detail, banner id: {0}", id);
            }
            finally
            {
                _logger.LogInformation("Get banner detail, banner Id {0}", id);
            }
            return Ok(new Response<Banner>("Has error"));
        }

        // POST api/<BannerController>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] SetBannerViewModel bannerModel)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage);

                return Ok(new Response { Successful = false, Message = string.Join(",", errorMessage) });
            }
            var createCommand = new CreateBannerCommand()
            {
                Model = bannerModel,
                CreateBy = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value
            };

            return Ok(await Mediator.Send(createCommand));
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromForm] UpdateBannerViewModel model)
        {
            bool isActionSuccess = false;
            try
            {
                var updateCommand = new UpdateBannerCommand()
                {
                    Model = model,
                    UpdateBy = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value
                };
                return Ok(await Mediator.Send(updateCommand));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update banner, data: {0}", model);
            }
            finally
            {
                _logger.LogInformation("Update banner, data: {0}, Result status: {1}", model, isActionSuccess);
            }
            return Ok(new { Successful = false, Error = "Has error when update banner" });
        }


        [HttpPost]
        [Route("delete")]
        public async Task<IActionResult> Delete(string Id)
        {
            try
            {
                var deleteCommand = new DeleteBannerCommand()
                {
                    Id = Id
                };
                return Ok(await Mediator.Send(deleteCommand));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete Banner, Id: {0}", Id);
            }
            finally
            {
                _logger.LogInformation("Delete Banner, Id: {0}", Id);
            }
            return Ok(new { Successful = false, Error = "Has error when delete" });
        }

    }
}