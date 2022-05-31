using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Tastee.Application.Features.Categories.Commands;
using Tastee.Application.Features.Categories.Queries;
using Tastee.Domain.Entities;
using Tastee.Shared;
using Tastee.Shared.Models.Categories;

namespace Tastee.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : BaseApiController
    {
        private readonly ILogger<CategoryController> _logger;
        public CategoryController(ILogger<CategoryController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("load-data")]
        public async Task<IActionResult> LoadData([FromForm] GetCategoriesViewModel model)
        {
            try
            {
                int recordsTotal = 0;
                GetCategoriesQuery groupQuery = new GetCategoriesQuery { RequestModel = model };
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
                    new { model.Draw, recordsFiltered = 0, recordsTotal = 0, data = new List<Category>() });
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] InsertCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage);

                return Ok(new Response { Successful = false, Message = string.Join(",", errorMessage) });
            }
            var createCommand = new CreateCategoryCommand()
            {
                Model = model,
                CreateBy = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value
            };
            
            return Ok(await Mediator.Send(createCommand));
        }

        [HttpPost]
        [Route("delete")]
        public async Task<IActionResult> Category_Delete(string Id)
        {
            try
            {
                var deleteCommand = new DeleteCategoryCommand()
                {
                    Id = Id
                };
                return Ok(await Mediator.Send(deleteCommand));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete category, Id: {0}", Id);
            }
            finally
            {
                _logger.LogInformation("Delete category, Id: {0}", Id);
            }
            return Ok(new { Successful = false, Error = "Has error when delete" });
        }
    }
}
