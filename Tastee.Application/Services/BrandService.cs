using LinqKit;
using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Domain.Entities;
using Tastee.Infrastucture.Data.Context;
using Tastee.Shared;
using URF.Core.Abstractions;

namespace Tastee.Application.Services
{
    public class BrandService : IBrandService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<BrandService> _logger;

        private readonly IGenericService<Brands> _serviceBrands;

        public BrandService(
           ILogger<BrandService> logger,
           IUnitOfWork unitOfWork,
           IGenericService<Brands> serviceBrands
           )
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _serviceBrands = serviceBrands;
        }

        public async Task<Brand> GetByIdAsync(string id)
        {
            var brand = await _serviceBrands.FindAsync(id);
            return brand.Adapt<Brand>();
        }

        public async Task<PaggingModel<Brand>> GetBrandsAsync(int pageSize, int? pageIndex, string name)
        {
            ExpressionStarter<Brands> searchCondition = PredicateBuilder.New<Brands>(true);

            if (name != null && name.Length > 0)
            {
                searchCondition = searchCondition.And(x => x.Name.ToLower().Contains(name.ToLower()));
            }

            var listBrands = _serviceBrands.Queryable().Where(searchCondition).OrderByDescending(x => x.CreatedDate);

            var pagedListUser = await PaginatedList<Brands>.CreateAsync(listBrands, pageIndex ?? 1, pageSize);

            PaggingModel<Brand> returnResult = new PaggingModel<Brand>()
            {
                ListData = pagedListUser.Adapt<List<Brand>>(),
                TotalRows = pagedListUser.TotalRows,
            };

            return returnResult;
        }

        public async Task<Response> InsertAsync(Brand model)
        {
            if (!_serviceBrands.Queryable().Any(x => x.Name == model.Name))
            {
                Brands newBrands = model.Adapt<Brands>();
                newBrands.Id = Guid.NewGuid().ToString();
                newBrands.Status = BrandStatus.Pending.ToString();
                newBrands.CreatedDate = DateTime.Now;
                newBrands.UpdatedDate = DateTime.Now;
                _serviceBrands.Insert(newBrands);
                await _unitOfWork.SaveChangesAsync();
                return new Response { Successful = true, Message = "Add brand successed" };
            }
            return new Response { Successful = false, Message = "Brand is exists" };
        }

        public async Task<Response> UpdateAsync(Brand model)
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
                    brand.Phone = model.Phone ?? brand.Phone;
                    brand.HeadOffice = model.HeadOffice ?? brand.HeadOffice;
                    brand.Uri = model.Uri ?? brand.Uri;
                    brand.Logo = model.Logo ?? brand.Logo;
                    brand.RestaurantImages = model.RestaurantImages ?? brand.RestaurantImages;
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

                    return new Response {Successful=true,Message="Update Brand success" }  ;
                }
                else
                {
                    return new Response { Successful = false, Message = "Brand not found" };
                }
            }

            return new Response { Successful = false, Message = "Please input id" };
        }
    }
}
