using LinqKit;
using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Application.Utilities;
using Tastee.Application.ViewModel;
using Tastee.Domain.Entities;
using Tastee.Infrastucture.Data.Context;
using Tastee.Shared;
using Tastee.Shared.Models.Brands;
using Tastee.Shared.Models.Brands.BrandDecorations;
using URF.Core.Abstractions;

namespace Tastee.Application.Services
{
    public class BrandService : IBrandService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<BrandService> _logger;

        private readonly IGenericService<Brands> _serviceBrands;
        private readonly IGenericService<BrandImages> _serviceBrandImage;
        private readonly IGenericService<BrandDecorations> _serviceBrandDecoration;

        public BrandService(
           ILogger<BrandService> logger,
           IUnitOfWork unitOfWork,
           IGenericService<Brands> serviceBrands,
           IGenericService<BrandImages> serviceBrandImage,
           IGenericService<BrandDecorations> serviceBrandDecoration
           )
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _serviceBrands = serviceBrands;
            _serviceBrandImage = serviceBrandImage;
            _serviceBrandDecoration = serviceBrandDecoration;
        }

        #region Brand
        public async Task<Brands> GetByIdAsync(string id)
        {
            var brand = await _serviceBrands.FindAsync(id);
            return brand;
        }

        public async Task<PaggingModel<Brand>> GetBrandsAsync(GetBrandsViewModel requestModel)
        {
            ExpressionStarter<Brands> searchCondition = PredicateBuilder.New<Brands>(true);
            int pageSize = Converters.StringToInteger(requestModel.Length, Constants.DEFAULT_PAGE_SIZE).Value;
            int skip = Converters.StringToInteger(requestModel.Start).Value;
            int pageIndex = skip / pageSize + 1;

            if (!string.IsNullOrEmpty(requestModel.Name))
            {
                searchCondition = searchCondition.And(x => x.Name.ToLower().Contains(requestModel.Name.ToLower()));
            }

            if (!string.IsNullOrEmpty(requestModel.Hotline))
            {
                searchCondition = searchCondition.And(x => x.Hotline.Contains(requestModel.Hotline));
            }

            if (!string.IsNullOrEmpty(requestModel.Email))
            {
                searchCondition = searchCondition.And(x => x.Email != null && x.Email.ToLower().Contains(requestModel.Email.ToLower()));
            }

            if (!string.IsNullOrEmpty(requestModel.City))
            {
                searchCondition = searchCondition.And(x => x.City != null && x.City.ToLower().Contains(requestModel.City.ToLower()));
            }

            if ((requestModel.Type ?? 0) != 0)
            {
                searchCondition = searchCondition.And(x => x.Type == requestModel.Type.Value);
            }

            if (!string.IsNullOrEmpty(requestModel.Status))
            {
                searchCondition = searchCondition.And(x => x.Status.ToLower() == requestModel.Status.ToLower());
            }

            DateTime? startDate = Converters.UnixTimeStampToDateTime(requestModel.StartDate);
            if (startDate != null)
            {
                searchCondition = searchCondition.And(x => x.StartDate != null && x.StartDate >= startDate.Value.Date);
            }
            DateTime? endDate = Converters.UnixTimeStampToDateTime(requestModel.EndDate);
            if (endDate != null)
            {
                endDate = endDate.Value.AddDays(1);
                searchCondition = searchCondition.And(x => x.EndDate != null && x.EndDate < endDate.Value.Date);
            }

            var listBrands = _serviceBrands.Queryable().Where(searchCondition).OrderByDescending(x => x.CreatedDate);

            var pagedListUser = await PaginatedList<Brands>.CreateAsync(listBrands, pageIndex, pageSize);

            PaggingModel<Brand> returnResult = new PaggingModel<Brand>()
            {
                ListData = pagedListUser.Select(x => BuildModelFromBrand(x)).ToList(),
                TotalRows = pagedListUser.TotalRows,
            };

            return returnResult;
        }

        public async Task<Response> InsertAsync(Brands newBrands)
        {
            if (!_serviceBrands.Queryable().Any(x => x.Name.ToLower() == newBrands.Name.ToLower()))
            {
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

        public async Task<Response> UpdateAsync(Brands updateBrand)
        {
            if (updateBrand.Id != null && updateBrand.Id.Length > 0)
            {
                var brand = await _serviceBrands.FindAsync(updateBrand.Id);
                if (brand == null)
                    return new Response { Successful = false, Message = "Brand not found" };
                if (_serviceBrands.Queryable().Any(x => x.Uri.ToLower() == updateBrand.Uri.ToLower() && x.Id != brand.Id))
                    return new Response { Successful = false, Message = "Uri is exists" };
                brand.Name = updateBrand.Name ?? brand.Name;
                brand.Address = updateBrand.Address ?? brand.Address;
                brand.Hotline = updateBrand.Hotline ?? brand.Hotline;
                brand.Email = updateBrand.Email ?? brand.Email;
                brand.Phone = updateBrand.Phone ?? brand.Phone;
                brand.HeadOffice = updateBrand.HeadOffice ?? brand.HeadOffice;
                brand.Uri = updateBrand.Uri ?? brand.Uri;
                brand.Logo = updateBrand.Logo ?? brand.Logo;
                brand.RestaurantImages = updateBrand.RestaurantImages ?? brand.RestaurantImages;
                brand.City = updateBrand.City ?? brand.City;
                brand.Area = updateBrand.Area ?? brand.Area;
                brand.MinPrice = updateBrand.MinPrice ?? brand.MinPrice;
                brand.MaxPrice = updateBrand.MaxPrice ?? brand.MaxPrice;
                brand.Status = updateBrand.Status ?? brand.Status;
                brand.UpdateBy = updateBrand.UpdateBy ?? brand.UpdateBy;
                brand.MetaDescription = updateBrand.MetaDescription ?? brand.MetaDescription;
                brand.SeoTitle = updateBrand.SeoTitle ?? brand.SeoTitle;
                brand.SeoDescription = updateBrand.SeoDescription ?? brand.SeoDescription;
                brand.SeoImage = updateBrand.SeoImage ?? brand.SeoImage;
                brand.Latitude = updateBrand.Latitude ?? brand.Latitude;
                brand.Longitude = updateBrand.Longitude ?? brand.Longitude;
                brand.Cuisines = updateBrand.Cuisines ?? brand.Cuisines;
                brand.Categories = updateBrand.Categories ?? brand.Categories;
                brand.MerchantId = updateBrand.MerchantId ?? brand.MerchantId;
                brand.OpenTimeA = updateBrand.OpenTimeA ?? brand.OpenTimeA;
                brand.CloseTimeA = updateBrand.CloseTimeA ?? brand.CloseTimeA;
                brand.OpenTimeP = updateBrand.OpenTimeP ?? brand.OpenTimeP;
                brand.CloseTimeP = updateBrand.CloseTimeP ?? brand.CloseTimeP;
                brand.StartDate = updateBrand.StartDate ?? brand.StartDate;
                brand.EndDate = updateBrand.EndDate ?? brand.EndDate;
                brand.Type = updateBrand.Type ?? brand.Type;
                brand.UpdatedDate = DateTime.Now;
                brand.UpdateBy = updateBrand.UpdateBy;
                _serviceBrands.Update(brand);

                await _unitOfWork.SaveChangesAsync();

                return new Response { Successful = true, Message = "Update Brand success" };
            }

            return new Response { Successful = false, Message = "Please input id" };
        }

        public Brands BuildBrandFromModel(Brand model)
        {
            var brand = new Brands()
            {
                Id = model.Id,
                Name = model.Name,
                Address = model.Address,
                Hotline = model.Hotline,
                Email = model.Email,
                Phone = model.Phone,
                HeadOffice = model.HeadOffice,
                Uri = model.Uri,
                Logo = model.Logo,
                RestaurantImages = model.RestaurantImages,
                City = model.City,
                Area = model.Area,
                MinPrice = model.MinPrice,
                MaxPrice = model.MaxPrice,
                Status = model.Status,
                UpdateBy = model.UpdateBy,
                MetaDescription = model.MetaDescription,
                SeoTitle = model.SeoTitle,
                SeoDescription = model.SeoDescription,
                SeoImage = model.SeoImage,
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                MerchantId = model.MerchantId,
                OpenTimeA = Converters.StringToTimeSpan(model.OpenTimeA),
                CloseTimeA = Converters.StringToTimeSpan(model.CloseTimeA),
                OpenTimeP = Converters.StringToTimeSpan(model.OpenTimeP),
                CloseTimeP = Converters.StringToTimeSpan(model.CloseTimeP),
                StartDate = Converters.UnixTimeStampToDateTime(model.StartDate),
                EndDate = Converters.UnixTimeStampToDateTime(model.EndDate),
                Type = model.Type,
                UpdatedDate = Converters.UnixTimeStampToDateTime(model.UpdatedDate),
                CreatedDate = Converters.UnixTimeStampToDateTime(model.CreatedDate, zeroIsNull: false).Value,
            };

            if (model.Cuisines == null)
                brand.Cuisines = null;
            else if (model.Cuisines.Length == 0)
                brand.Cuisines = String.Empty;
            else brand.Cuisines = String.Join(',', model.Cuisines);


            if (model.Categories == null)
                brand.Categories = null;
            else if (model.Categories.Length == 0)
                brand.Categories = String.Empty;
            else brand.Categories = String.Join(',', model.Categories);

            return brand;
        }

        public Brand BuildModelFromBrand(Brands brand)
        {
            var model = new Brand()
            {
                Id = brand.Id,
                Name = brand.Name,
                Address = brand.Address,
                Hotline = brand.Hotline,
                Email = brand.Email,
                Phone = brand.Phone,
                HeadOffice = brand.HeadOffice,
                Uri = brand.Uri,
                Logo = brand.Logo,
                RestaurantImages = brand.RestaurantImages,
                City = brand.City,
                Area = brand.Area,
                MinPrice = brand.MinPrice,
                MaxPrice = brand.MaxPrice,
                Status = brand.Status,
                UpdateBy = brand.UpdateBy,
                MetaDescription = brand.MetaDescription,
                SeoTitle = brand.SeoTitle,
                SeoDescription = brand.SeoDescription,
                SeoImage = brand.SeoImage,
                Latitude = brand.Latitude,
                Longitude = brand.Longitude,
                MerchantId = brand.MerchantId,
                OpenTimeA = Converters.TimeSpanToString(brand.OpenTimeA),
                CloseTimeA = Converters.TimeSpanToString(brand.CloseTimeA),
                OpenTimeP = Converters.TimeSpanToString(brand.OpenTimeP),
                CloseTimeP = Converters.TimeSpanToString(brand.CloseTimeP),
                StartDate = Converters.DateTimeToUnixTimeStamp(brand.StartDate),
                EndDate = Converters.DateTimeToUnixTimeStamp(brand.EndDate),
                Type = brand.Type,
                UpdatedDate = Converters.DateTimeToUnixTimeStamp(brand.UpdatedDate),
                CreatedDate = Converters.DateTimeToUnixTimeStamp(brand.CreatedDate).Value,
            };
            if (!String.IsNullOrEmpty(brand.Cuisines))
                model.Cuisines = brand.Cuisines.Split(',');
            else model.Cuisines = new string[0];

            if (!String.IsNullOrEmpty(brand.Categories))
                model.Categories = brand.Categories.Split(',');
            else model.Categories = new string[0];

            return model;
        }
        #endregion

        #region RestaurantSpace
        public async Task<Response> InsertBrandImagesAsync(List<BrandImages> listRestaurantSpace)
        {
            foreach (var item in listRestaurantSpace)
            {
                _serviceBrandImage.Insert(item);
            }
            await _unitOfWork.SaveChangesAsync();
            return new Response { Successful = true, Message = "Add successed" };
        }

        public async Task<Response> DeleteBrandImagesAsync(string id)
        {
            var image = await GetBrandImageByIdAsync(id);
            if (image != null)
                _serviceBrandImage.Delete(image);

            await _unitOfWork.SaveChangesAsync();
            return new Response { Successful = true, Message = "Delete successed" };
        }

        public async Task<PaggingModel<BrandImage>> GetBrandImagesAsync(GetBrandImagesModel requestModel)
        {
            ExpressionStarter<BrandImages> searchCondition = PredicateBuilder.New<BrandImages>(true);
            int pageSize = Converters.StringToInteger(requestModel.Length, Constants.DEFAULT_PAGE_SIZE).Value;
            int skip = Converters.StringToInteger(requestModel.Start).Value;
            int pageIndex = skip / pageSize + 1;

            if (!string.IsNullOrEmpty(requestModel.ID))
            {
                searchCondition = searchCondition.And(x => x.Id == requestModel.ID);
            }

            if ((requestModel.Status ?? 0) != 0)
            {
                searchCondition = searchCondition.And(x => x.Status != null && x.Status == requestModel.Status.Value);
            }

            if (!string.IsNullOrEmpty(requestModel.BrandID))
            {
                searchCondition = searchCondition.And(x => x.BrandId == requestModel.BrandID);
            }

            var listBrandImages = _serviceBrandImage.Queryable().Where(searchCondition).OrderByDescending(x => x.CreatedDate);

            var pagedListUser = await PaginatedList<BrandImages>.CreateAsync(listBrandImages, pageIndex, pageSize);

            PaggingModel<BrandImage> returnResult = new PaggingModel<BrandImage>()
            {
                ListData = pagedListUser.Select(x => BuildModelFromBrandImage(x)).ToList(),
                TotalRows = pagedListUser.TotalRows,
            };

            return returnResult;
        }

        public async Task<BrandImages> GetBrandImageByIdAsync(string id)
        {
            var image = await _serviceBrandImage.FindAsync(id);
            return image;
        }

        public async Task<Response> UpdateBrandImageAsync(BrandImages updateImage)
        {
            if (updateImage.Id != null && updateImage.Id.Length > 0)
            {
                var image = await _serviceBrandImage.FindAsync(updateImage.Id);
                if (image != null)
                {
                    image.Status = updateImage.Status ?? image.Status;
                    image.Description = updateImage.Description;
                    image.UpdatedDate = Converters.DateTimeToUnixTimeStamp(DateTime.Now).Value;
                    image.UpdatedBy = updateImage.UpdatedBy;
                    _serviceBrandImage.Update(image);

                    await _unitOfWork.SaveChangesAsync();

                    return new Response { Successful = true, Message = "Update BrandImage success" };
                }
                else
                {
                    return new Response { Successful = false, Message = "BrandImage not found" };
                }
            }

            return new Response { Successful = false, Message = "Please input id" };
        }


        public BrandImage BuildModelFromBrandImage(BrandImages brandImage)
        {
            var model = new BrandImage()
            {
                Id = brandImage.Id,
                BrandId = brandImage.BrandId,
                Description = brandImage.Description,
                Image = brandImage.Image,
                Status = brandImage.Status,
                UpdatedDate = brandImage.UpdatedDate,
                UpdatedBy = brandImage.UpdatedBy,
                CreatedDate = brandImage.CreatedDate,
                CreatedBy = brandImage.CreatedBy,
            };
            return model;
        }

        public BrandImages BuildBrandImageFromModel(BrandImage model)
        {
            var image = new BrandImages()
            {
                Id = model.Id,
                BrandId = model.BrandId,
                Description = model.Description,
                Image = model.Image,
                Status = model.Status,
                UpdatedDate = model.UpdatedDate,
                UpdatedBy = model.UpdatedBy,
                CreatedDate = model.CreatedDate,
                CreatedBy = model.CreatedBy,
            };
            return image;
        }

        #endregion

        #region BrandDecoration
        public async Task<Response> InsertBrandDecorationAsync(BrandDecorations item)
        {
            item.CreatedDate = Converters.DateTimeToUnixTimeStamp(DateTime.Now).Value;
            item.Id = Guid.NewGuid().ToString();
            _serviceBrandDecoration.Insert(item);
            await _unitOfWork.SaveChangesAsync();
            return new Response { Successful = true, Message = "Add successed" };
        }

        public async Task<BrandDecorations> GetBrandDecorationByBrandIdAsync(string brandId)
        {
            ExpressionStarter<BrandDecorations> searchCondition = PredicateBuilder.New<BrandDecorations>(true);
            searchCondition = searchCondition.And(x => x.BrandId == brandId);
            return _serviceBrandDecoration.Queryable().Where(searchCondition).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
        }

        public WidgetsModel BuildDefaultBrandDecoration(Brands brand)
        {
            WidgetsModel widgets = new WidgetsModel
            {
                InfoWidget = new InfoWidgetModel
                {
                    BrandAddress = brand.Address,
                    BrandImage = brand.RestaurantImages,
                    BrandLogo = brand.Logo,
                    BrandName = brand.Name,
                    BrandType = brand.Type,
                    DisplayOrder = 0,
                    Style = 1,
                    Url= brand.Uri
                }

            };
            return widgets;
        }



        #endregion
    }
}
