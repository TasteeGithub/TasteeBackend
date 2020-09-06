using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqKit;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGeneration;
using TTBackEnd.Shared;
using TTFrontEnd.Models.DataContext;
using TTFrontEnd.Services;
using URF.Core.Abstractions;

namespace TTFrontEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<BrandsController> _logger;

        private readonly ITTService<Brands> _serviceBrands;

        public BrandsController(
            ILogger<BrandsController> logger,
            IUnitOfWork unitOfWork,
            ITTService<Brands> serviceBrands
            )
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _serviceBrands = serviceBrands;
        }

        [HttpPost]
        [Route("load-data")]
        public async Task<IActionResult> LoadData(
            [FromForm] string draw,
            [FromForm] string start,
            [FromForm] string length
            ,[FromForm] string name
            //,
            //[FromForm] string email,
            //[FromForm] string phone,
            //[FromForm] DateTime? fromDate,
            //[FromForm] DateTime? toDate,
            //[FromForm] string status
            )
        {
            try
            {
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int pageIndex = skip / pageSize + 1;
                int recordsTotal = 0;

                //var rs = await Get(pageSize, pageIndex, name, email, phone, status, fromDate, toDate);
                var rs = await Get(pageSize, pageIndex, name);

                //total number of rows counts
                recordsTotal = rs.TotalRows;

                //Paging
                var data = rs.ListData; //customerData.Skip(skip).Take(pageSize).ToList();

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
        // GET: api/Brands
        [HttpGet]
        public async Task<PaggingModel<Brands>> Get(int pageSize, int? pageIndex,string name)
        {
            ExpressionStarter<Brands> searchCondition = PredicateBuilder.New<Brands>(true);

            if (name != null && name.Length > 0)
            {
                searchCondition = searchCondition.And(x => x.Name.ToLower().Contains(name.ToLower()));
            }

            var listBrands = _serviceBrands.Queryable().Where(searchCondition).OrderByDescending(x => x.CreatedDate);

            var pagedListUser = await PaginatedList<Brands>.CreateAsync(listBrands, pageIndex ?? 1, pageSize);

            PaggingModel<Brands> returnResult = new PaggingModel<Brands>()
            {
                ListData = pagedListUser.Adapt<List<Brands>>(),
                TotalRows = pagedListUser.TotalRows,
            };

            return returnResult;
        }

        // GET: api/Brands/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<Brands> Get(string id)
        {
            try
            {
                var brand = await _serviceBrands.FindAsync(id);
                return brand;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get brand detail, brand id: {0}", id);
            }
            finally
            {
                _logger.LogInformation("Get brand detail, brand Id {0}", id);
            }
            return null;

        }

        // POST: api/Brands
        [HttpPost]
        public async Task<IActionResult> Post(BrandModel brandModel)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage);

                return Ok(new RegisterResult { Successful = false, Error = errorMessage });
            }

            if (!_serviceBrands.Queryable().Any(x => x.Name == brandModel.Name))
            {
                Brands newBrands = brandModel.Adapt<Brands>();
                newBrands.Id = Guid.NewGuid().ToString();
                newBrands.Status = BrandsStatus.Pending.ToString();
                newBrands.CreatedDate = DateTime.Now;
                newBrands.UpdatedDate = DateTime.Now;
                _serviceBrands.Insert(newBrands);
                await _unitOfWork.SaveChangesAsync();
                return Ok(new RegisterResult { Successful = true });
            }

            return Ok(new RegisterResult { Successful = false, Error = new string[] { "Brand is exists" } });
        }

        // PUT: api/Brands/5
        [HttpPut]
        public async Task<IActionResult> Put(Brands model)
        {
            bool isActionSuccess = false;
            try
            {
                if (model.Id != null && model.Id.Length > 0)
                {

                    var brand = await _serviceBrands.FindAsync(model.Id);
                    if (brand != null)
                    {
                        brand.Name = model.Name ?? brand.Name;
                        brand.Address = model.Address ?? brand.Address;
                        brand.Hotline = model.Hotline ?? brand.Hotline;
                        brand.Email = model.Email ?? brand.Email;
                        brand.Phone = model.Logo ?? brand.Phone;
                        brand.HeadOffice = model.HeadOffice ?? brand.HeadOffice;
                        brand.Uri = model.Uri ?? brand.Uri;
                        brand.Logo = model.Logo ?? brand.Logo;
                        brand.City = model.City ?? brand.City;
                        brand.Area = model.Area ?? brand.Area;
                        brand.MinPrice = model.MinPrice ?? brand.MinPrice;
                        brand.MaxPrice = model.MaxPrice ?? brand.MaxPrice;
                        brand.Status = model.Status ?? brand.Status;
                        brand.UpdateBy = model.UpdateBy ?? brand.UpdateBy;
                        brand.MetaDescription = model.MetaDescription ?? brand.MetaDescription;
                        brand.SeoTitle = model.SeoTitle ?? brand.SeoTitle;
                        brand.SeoDescription = model.SeoDescription ?? brand.SeoDescription;
                        brand.SeoImage = model.SeoImage ?? brand.SeoImage;
                        brand.Latitude = model.Latitude ?? brand.Latitude;
                        brand.Longitude = model.Longitude ?? brand.Longitude;
                        brand.Cuisines = model.Cuisines ?? brand.Cuisines;
                        brand.Categories = model.Categories ?? brand.Categories;
                        brand.MerchantId = model.MerchantId ?? brand.MerchantId;

                        _serviceBrands.Update(brand);

                        await _unitOfWork.SaveChangesAsync();
                        isActionSuccess = true;
                        return Ok(new { Successful = true });
                    }
                    else
                    {
                        isActionSuccess = true;
                        return Ok(new { Successful = false, Error = "brand not found" });
                    }
                }
                isActionSuccess = true;
                return Ok(new { Successful = false, Error = "Please input id" });
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
