using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Tastee.Application.Features.Brands.BrandDecorationFeatures.Commands;
using Tastee.Application.Features.Brands.BrandImagesFeatures.Commands;
using Tastee.Application.Features.Brands.BrandImagesFeatures.Queries;
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
using Tastee.Shared.Models.Brands;
using Tastee.Shared.Models.Brands.BrandDecorations;

namespace Tastee.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : BaseApiController
    {
        private readonly ILogger<BrandsController> _logger;

        #region Contructor
        public BrandsController(
            ILogger<BrandsController> logger
            )
        {
            _logger = logger;
        }
        #endregion

        #region Brand
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
        #endregion

        #region BrandImages
        [HttpPost]
        [Route("images")]
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

        /// <summary>
        /// Get list BrandImage
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("images/load-data")]
        public async Task<IActionResult> BrandImages_LoadData([FromForm] GetBrandImagesModel model)
        {
            try
            {
                int recordsTotal = 0;
                GetBrandImagesQuery brandsQuery = new GetBrandImagesQuery { RequestModel = model };
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
                    new { model.Draw, recordsFiltered = 0, recordsTotal = 0, data = new List<BrandImages>() });
        }

        /// <summary>
        /// Get Brand Image Detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("images/detail/{id}")]
        public async Task<IActionResult> GetBrandImageDetail(string id)
        {
            try
            {
                GetBrandImageByIdQuery rq = new GetBrandImageByIdQuery { Id = id };
                return Ok(await Mediator.Send(rq));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get brand image detail, Id: {0}", id);
            }
            finally
            {
                _logger.LogInformation("Get brand image detail, Id {0}", id);
            }
            return Ok(new Response<Brand>("Has error"));
        }

        /// <summary>
        /// Update Brand Image
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("images/update")]
        public async Task<IActionResult> UpdateBrandImage(UpdateBrandImageModel model)
        {
            bool isActionSuccess = false;
            try
            {
                var updateCommand = new UpdateBrandImageCommand()
                {
                    Model = model,
                    UpdatedBy = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value
                };

                return Ok(await Mediator.Send(updateCommand));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update BrandImage, BrandImage: {0}", model);
            }
            finally
            {
                _logger.LogInformation("Update BrandImage, data: {0}, Result status: {1}", model, isActionSuccess);
            }
            return Ok(new { Successful = false, Error = "Has error when update" });
        }


        /// <summary>
        /// Delete Brand Image
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("images/delete")]
        public async Task<IActionResult> DeleteBrandImage(string id)
        {
            bool isActionSuccess = false;
            try
            {
                var deleteCommand = new DeleteBrandImageCommand()
                {
                    ID = id
                };

                return Ok(await Mediator.Send(deleteCommand));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete BrandImage, ID: {0}", id);
            }
            finally
            {
                _logger.LogInformation("Delete BrandImage, id: {0}, Result status: {1}", id, isActionSuccess);
            }
            return Ok(new { Successful = false, Error = "Has error when delete" });
        }
        #endregion

        #region Decoration
        /// <summary>
        /// Init Brand Decoration
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("decoration")]
        public async Task<IActionResult> BrandDecoration_Init(InitBrandDecorationModel model)
        {
            try
            {
                InitBrandDecorationCommand rq = new InitBrandDecorationCommand
                {

                    BrandId = model.BrandId,
                    UserEmail = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value
                };
                return Ok(await Mediator.Send(rq));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "init brand decoration, brand id: {0}", model.BrandId);
            }
            finally
            {
                _logger.LogInformation("init brand decoration, brand Id {0}", model.BrandId);
            }
            return Ok(new Response<Brand>("Has error"));
        }
        #endregion
    }
}