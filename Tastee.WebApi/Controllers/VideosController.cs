using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Tastee.Application.Features.Videos.Commands;
using Tastee.Application.Features.Videos.Queries;
using Tastee.Application.Wrappers;
using Tastee.Domain.Entities;
using Tastee.Shared;
using Tastee.Shared.Models.Videos;

namespace Tastee.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class VideosController : BaseApiController
    {
        private readonly ILogger<VideosController> _logger;
        public VideosController(ILogger<VideosController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("load-data")]
        public async Task<IActionResult> LoadData([FromForm] GetVideosViewModel model)
        {
            try
            {
                int recordsTotal = 0;
                GetVideosQuery groupQuery = new GetVideosQuery { RequestModel = model };
                var rs = await Mediator.Send(groupQuery);

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
                    new { model.Draw, recordsFiltered = 0, recordsTotal = 0, data = new List<VideoModel>() });
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] InsertVideoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage);

                return Ok(new Response { Successful = false, Message = string.Join(",", errorMessage) });
            }
            var createCommand = new CreateVideoCommand()
            {
                Model = model,
                CreateBy = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value
            };

            return Ok(await Mediator.Send(createCommand));
        }

        [HttpPost]
        [Route("delete")]
        public async Task<IActionResult> Video_Delete(string Id)
        {
            try
            {
                var deleteCommand = new DeleteVideoCommand()
                {
                    Id = Id
                };
                return Ok(await Mediator.Send(deleteCommand));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete video, Id: {0}", Id);
            }
            finally
            {
                _logger.LogInformation("Delete video, Id: {0}", Id);
            }
            return Ok(new { Successful = false, Error = "Has error when delete" });
        }

        /// <summary>
        /// Update Category
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromForm] UpdateVideoViewModel model)
        {
            bool isActionSuccess = false;
            try
            {
                var updateCommand = new UpdateVideoCommand()
                {
                    VideoModel = model,
                    UpdateBy = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value
                };
                return Ok(await Mediator.Send(updateCommand));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update video, data: {0}", model);
            }
            finally
            {
                _logger.LogInformation("Update video, data: {0}, Result status: {1}", model, isActionSuccess);
            }
            return Ok(new { Successful = false, Error = "Has error when update video" });
        }
    }
}
