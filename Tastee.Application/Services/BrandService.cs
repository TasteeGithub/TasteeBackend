using LinqKit;
using Mapster;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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

        private readonly IGenericService<Brands> _serviceBrands;
        private readonly IGenericService<BrandImages> _serviceBrandImage;
        private readonly IGenericService<BrandDecorations> _serviceBrandDecoration;
        private readonly IGenericService<WidgetImages> _serviceWidgetImages;
        private readonly IGenericService<Menus> _serviceMenus;
        private readonly IGenericService<MenuItems> _serviceMenuItems;
        private readonly IGenericService<BrandMerchants> _serviceBrandMerchants;
        private readonly IGenericService<Users> _serviceUsers;
        private readonly IGenericService<Widgets> _serviceWidgets;

        public BrandService(
           IUnitOfWork unitOfWork,
           IGenericService<Brands> serviceBrands,
           IGenericService<BrandImages> serviceBrandImage,
           IGenericService<BrandDecorations> serviceBrandDecoration,
           IGenericService<WidgetImages> serviceWidgetImages,
           IGenericService<Menus> serviceMenus,
           IGenericService<MenuItems> serviceMenuItems,
           IGenericService<BrandMerchants> serviceBrandMerchants,
           IGenericService<Users> serviceUsers,
           IGenericService<Widgets> serviceWidgets
           )
        {
            _unitOfWork = unitOfWork;
            _serviceBrands = serviceBrands;
            _serviceBrandImage = serviceBrandImage;
            _serviceBrandDecoration = serviceBrandDecoration;
            _serviceMenus = serviceMenus;
            _serviceMenuItems = serviceMenuItems;
            _serviceWidgetImages = serviceWidgetImages;
            _serviceBrandMerchants = serviceBrandMerchants;
            _serviceUsers = serviceUsers;
            _serviceWidgets = serviceWidgets;
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


                var user = _serviceUsers.Queryable().Where(x => x.Email == newBrands.UpdateBy).FirstOrDefault();
                if (user != null)
                {
                    var brandMerchants = new BrandMerchants()
                    {
                        BrandId = newBrands.Id,
                        UserId = user.Id,
                        Id = Guid.NewGuid().ToString()
                    };
                    _serviceBrandMerchants.Insert(brandMerchants);
                }
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
                if (!String.IsNullOrEmpty(updateBrand.Uri))
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
                brand.WebMap = updateBrand.WebMap ?? brand.WebMap;
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
                WebMap = model.WebMap,
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
                WebMap = brand.WebMap,
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

        public async Task<Response> CheckMenuItemsBelongBrand(List<string> itemIds, string brandId)
        {
            var brand = await GetByIdAsync(brandId);
            if (brand == null)
                return new Response() { Successful = false, Message = String.Format("brand not found") };

            var notExistIds = itemIds.Except(_serviceMenuItems.Queryable().Select(x => x.Id)).ToList();
            if (notExistIds.Count != 0)
                return new Response() { Successful = false, Message = String.Format("Item: {0} not found", notExistIds.First()) };

            var menuIds = _serviceMenuItems.Queryable().Where(x => itemIds.Contains(x.Id)).Select(x => x.MenuId).ToList();
            var brandIdsFromMenu = _serviceMenus.Queryable().Where(x => menuIds.Contains(x.Id)).Select(x => x.BrandId).ToList();
            if (brandIdsFromMenu.Count != 1 || brandIdsFromMenu.FirstOrDefault() != brandId)
                return new Response() { Successful = false, Message = String.Format("Items not belong brand") };
            return new Response() { Successful = true };
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
        /// <summary>
        /// InsertBrandDecorationAsync
        /// </summary>
        /// <param name="model"></param>
        /// <param name="brandId"></param>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        public async Task<Response> InsertBrandDecorationAsync(WidgetsModel model, string brandId, string userEmail)
        {
            var decoration = new BrandDecorations
            {
                BrandId = brandId,
                CreatedBy = userEmail,
                Status = (int)BrandDecorationStatus.Draft,
            };
            decoration.CreatedDate = Converters.DateTimeToUnixTimeStamp(DateTime.Now).Value;
            decoration.Id = Guid.NewGuid().ToString();
            _serviceBrandDecoration.Insert(decoration);

            var infoWidget = new Widgets()
            {
                DecorationId = decoration.Id,
                Id = Guid.NewGuid().ToString(),
                ExtraData = JsonConvert.SerializeObject(model.InfoWidget),
                WidgetType = (int)WidgetType.Info
            };
            _serviceWidgets.Insert(infoWidget);

            await _unitOfWork.SaveChangesAsync();
            return new Response { Successful = true, Message = "Add successed" };
        }

        /// <summary>
        /// UpdateBrandDecorationAsync
        /// </summary>
        /// <param name="item"></param>
        /// <param name="widgets"></param>
        /// <returns></returns>
        public async Task<Response> UpdateBrandDecorationAsync(BrandDecorations item, WidgetsModel widgets)
        {
            if (item.Id != null && item.Id.Length > 0)
            {
                var decoration = await _serviceBrandDecoration.FindAsync(item.Id);
                if (decoration != null)
                {
                    decoration.Status = item.Status;
                    decoration.UpdatedDate = Converters.DateTimeToUnixTimeStamp(DateTime.Now);
                    decoration.UpdatedBy = item.UpdatedBy;
                    _serviceBrandDecoration.Update(decoration);

                    //Widget
                    var summary = BuildWidgetsSummary(widgets, decoration.Id);
                    //Clear old data
                    var dWidgets = _serviceWidgets.Queryable().Where(x => x.DecorationId == decoration.Id).ToList();
                    var widgetIds = dWidgets.Select(x => x.Id).ToList();
                    var images = _serviceWidgetImages.Queryable().Where(x => widgetIds.Contains(x.WidgetId)).ToList();
                    foreach (var img in images)
                        _serviceWidgetImages.Delete(img);
                    foreach (var widget in dWidgets)
                        _serviceWidgets.Delete(widget);
                    //Insert new data
                    foreach (var widget in summary.Widgets)
                    {
                        _serviceWidgets.Insert(widget);
                        if(widget.WidgetType == (int)WidgetType.Info)
                        {
                            var brand = _serviceBrands.Queryable().Where(x => x.Id == decoration.BrandId).FirstOrDefault();
                            widget.ExtraData.TryParseJson(out InfoWidgetModel infomodel);
                            brand.RestaurantImages = infomodel.BrandImage;
                        }
                    }
                  
                    foreach (var img in summary.Images)
                        _serviceWidgetImages.Insert(img);

                    await _unitOfWork.SaveChangesAsync();

                    return new Response { Successful = true, Message = "Update Brand Decoration success" };
                }
                else
                {
                    return new Response { Successful = false, Message = "Brand Decoration not found" };
                }
            }

            return new Response { Successful = false, Message = "Please input id" };
        }

        /// <summary>
        /// GetBrandDecorationByBrandId
        /// </summary>
        /// <param name="brandId"></param>
        /// <returns></returns>
        public BrandDecorations GetBrandDecorationByBrandId(string brandId)
        {
            ExpressionStarter<BrandDecorations> searchCondition = PredicateBuilder.New<BrandDecorations>(true);
            searchCondition = searchCondition.And(x => x.BrandId == brandId && x.Status == (int)BrandDecorationStatus.Draft);
            return _serviceBrandDecoration.Queryable().Where(searchCondition).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
        }

        /// <summary>
        /// BuildDefaultBrandDecoration
        /// </summary>
        /// <param name="brand"></param>
        /// <returns></returns>
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
                    Url = brand.Uri
                }

            };
            return widgets;
        }

        /// <summary>
        /// BuildBrandDecoration
        /// </summary>
        /// <param name="brandDecoration"></param>
        /// <returns></returns>
        public WidgetsModel BuildBrandDecoration(BrandDecorations brandDecoration)
        {
            WidgetsModel model = new WidgetsModel();
            if (brandDecoration == null)
                return null;

            var brand = _serviceBrands.Queryable().Where(s => s.Id == brandDecoration.BrandId).FirstOrDefault();
            model = BuildDefaultBrandDecoration(brand);

            var listWidgets = _serviceWidgets.Queryable().Where(x => x.DecorationId == brandDecoration.Id).ToList();
            foreach (var widget in listWidgets)
            {
                if (widget.WidgetType == (int)WidgetType.Info)
                {
                    widget.ExtraData.TryParseJson(out InfoWidgetModel infomodel);
                    model.InfoWidget = infomodel;
                }
                else if (widget.WidgetType == (int)WidgetType.BrandImage)
                {
                    widget.ExtraData.TryParseJson(out GeneralWidgetModel brandImageModel);
                    model.BrandImageWidget = brandImageModel;
                }
                else if (widget.WidgetType == (int)WidgetType.GroupItem)
                {
                    widget.ExtraData.TryParseJson(out GroupItemWidgetModel groupItemModel);
                    if (model.GroupItemWidget == null)
                        model.GroupItemWidget = new List<GroupItemWidgetModel>();
                    model.GroupItemWidget.Add(groupItemModel);
                }
                else if (widget.WidgetType == (int)WidgetType.Menu)
                {
                    widget.ExtraData.TryParseJson(out MenuWidgetModel menuModel);
                    model.MenuWidget = menuModel;
                }
                else if (widget.WidgetType == (int)WidgetType.SingelBanner)
                {
                    widget.ExtraData.TryParseJson(out SingelBannerWidgetModel singerModel);
                    if (model.SingleBannerWidget == null)
                        model.SingleBannerWidget = new List<SingelBannerWidgetModel>();
                    model.SingleBannerWidget.Add(singerModel);
                }
                else if (widget.WidgetType == (int)WidgetType.SliderBanner)
                {
                    widget.ExtraData.TryParseJson(out SliderBannerWidgetModel sliderModel);
                    if (model.SliderBannerWidget == null)
                        model.SliderBannerWidget = new List<SliderBannerWidgetModel>();
                    model.SliderBannerWidget.Add(sliderModel);
                }
            }

            return model;
        }

        #region Widget
        /// <summary>
        /// GetWidgetByIdAsync
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Widgets> GetWidgetByIdAsync(string id)
        {
            var widget = await _serviceWidgets.FindAsync(id);
            return widget;
        }
        #endregion

        #region WidgetImages
        /// <summary>
        /// GetWidgetImageByIdAsync
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<WidgetImages> GetWidgetImageByIdAsync(string id)
        {
            var image = await _serviceWidgetImages.FindAsync(id);
            return image;
        }

        /// <summary>
        /// InsertWidgetImagesAsync
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public async Task<Response> InsertWidgetImagesAsync(List<WidgetImages> items)
        {
            foreach (var item in items)
            {
                _serviceWidgetImages.Insert(item);
            }
            await _unitOfWork.SaveChangesAsync();
            return new Response { Successful = true, Message = "Add successed" };
        }

        /// <summary>
        /// GetWidgetImageAsync
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public async Task<PaggingModel<WidgetImage>> GetWidgetImageAsync(GetWidgetImageModel requestModel)
        {
            ExpressionStarter<WidgetImages> searchCondition = PredicateBuilder.New<WidgetImages>(true);
            int pageSize = Converters.StringToInteger(requestModel.Length, Constants.DEFAULT_PAGE_SIZE).Value;
            int skip = Converters.StringToInteger(requestModel.Start).Value;
            int pageIndex = skip / pageSize + 1;

            List<string> DecorationIDs = new List<string>();
            List<string> WidgetIds = new List<string>();

            if (!string.IsNullOrEmpty(requestModel.BrandId))
            {
                DecorationIDs = _serviceBrandDecoration.Queryable().Where(x => x.BrandId == requestModel.BrandId).Select(x => x.Id).ToList();
            }
            if (!string.IsNullOrEmpty(requestModel.DecorationId))
            {
                DecorationIDs.Add(requestModel.DecorationId);
            }

            var list_type = new List<int>
            {
                (int)WidgetType.SingelBanner,
                (int)WidgetType.SliderBanner
            };
            var widget_query = _serviceWidgets.Queryable().Where(x => list_type.Contains(x.WidgetType));
            if (DecorationIDs.Count != 0)
            {
                WidgetIds = widget_query.Where(x => DecorationIDs.Contains(x.DecorationId)).Select(x => x.Id).ToList();
            }

            if (!string.IsNullOrEmpty(requestModel.WidgetId))
            {
                WidgetIds.Add(requestModel.WidgetId);
            }

            if (WidgetIds.Count != 0)
            {
                searchCondition = searchCondition.And(x => WidgetIds.Contains(x.WidgetId));
            }

            var listImages = _serviceWidgetImages.Queryable().Where(searchCondition);

            var pagedListImages = await PaginatedList<WidgetImages>.CreateAsync(listImages, pageIndex, pageSize);

            var listWidgets = new List<WidgetImage>();
            foreach (var item in pagedListImages)
            {
                listWidgets.Add(await BuildModelFromWidgetImagesAsync(item));
            }

            PaggingModel<WidgetImage> returnResult = new PaggingModel<WidgetImage>()
            {
                ListData = listWidgets,
                TotalRows = pagedListImages.TotalRows,
            };

            return returnResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Response> DeleteDecorationImageAsync(string id)
        {
            var image = await GetWidgetImageByIdAsync(id);
            if (image == null)
                return new Response { Successful = false, Message = "Image not found" };

            var widget = await GetWidgetByIdAsync(image.WidgetId);
            if (widget != null)
            {
                if (widget.WidgetType == (int)WidgetType.SingelBanner)
                {
                    widget.ExtraData.TryParseJson(out SingelBannerWidgetModel uWidgetModel);
                    uWidgetModel.Image = String.Empty;
                    await UpdateWidgetExtraDataAsync(widget.Id, JsonConvert.SerializeObject(uWidgetModel));
                }
                else if (widget.WidgetType == (int)WidgetType.SliderBanner)
                {
                    widget.ExtraData.TryParseJson(out SliderBannerWidgetModel uWidgetModel);
                    uWidgetModel.Images.Remove(image.Image);
                    await UpdateWidgetExtraDataAsync(widget.Id, JsonConvert.SerializeObject(uWidgetModel));
                }
                else
                {
                    return new Response { Successful = false, Message = "Image invalid" };
                }

            }
            await _unitOfWork.SaveChangesAsync();
            return new Response { Successful = true, Message = "Delete successed" };
        }

        /// <summary>
        /// UpdateDecorationImageAsync
        /// </summary>
        /// <param name="updateImage"></param>
        /// <returns></returns>
        public async Task<Response> UpdateDecorationImageAsync(UpdateDecorationImageModel updateImage)
        {
            if (updateImage.Id != null && updateImage.Id.Length > 0)
            {
                var image = await _serviceWidgetImages.FindAsync(updateImage.Id);
                if (image != null)
                {
                    image.Status = updateImage.Status;
                    _serviceWidgetImages.Update(image);

                    await _unitOfWork.SaveChangesAsync();

                    return new Response { Successful = true, Message = "Update Decoration Image success" };
                }
                else
                {
                    return new Response { Successful = false, Message = "Decoration Image not found" };
                }
            }

            return new Response { Successful = false, Message = "Please input id" };
        }

        #endregion
        #endregion

        #region BrandMerchants
        public async Task<Response> InsertBrandMerchantsAsync(BrandMerchants item)
        {
            _serviceBrandMerchants.Insert(item);
            await _unitOfWork.SaveChangesAsync();
            return new Response { Successful = true, Message = "Add successed" };
        }
        #endregion


        #region Private Method
        public class WidgetsSummary
        {
            public List<Widgets> Widgets { get; set; }
            public List<WidgetImages> Images { get; set; }
        }

        private async Task<WidgetImage> BuildModelFromWidgetImagesAsync(WidgetImages item)
        {
            var rs = new WidgetImage()
            {
                Id = item.Id,
                Image = item.Image,
                Status = item.Status,
                WidgetId = item.WidgetId
            };
            var widget = await GetWidgetByIdAsync(item.WidgetId);
            if (widget != null)
            {
                var decoration = _serviceBrandDecoration.Queryable().Where(x => x.Id == widget.DecorationId).FirstOrDefault();
                if (decoration != null)
                {
                    rs.DecorationId = decoration.Id;
                    rs.BrandId = decoration.BrandId;
                }
            }
            return rs;
        }

        private WidgetsSummary BuildWidgetsSummary(WidgetsModel model, string decorationId)
        {
            var rs = new WidgetsSummary()
            {
                Widgets = new List<Widgets>(),
                Images = new List<WidgetImages>(0)
            };

            //InfoWidget
            if (model.InfoWidget != null)
            {
                var info = new Widgets()
                {
                    DecorationId = decorationId,
                    Id = Guid.NewGuid().ToString(),
                    WidgetType = (int)WidgetType.Info,
                    ExtraData = JsonConvert.SerializeObject(model.InfoWidget)
                };
                var infoImage = new WidgetImages()
                {
                    Id = Guid.NewGuid().ToString(),
                    WidgetId = info.Id,
                    Image = model.InfoWidget.BrandImage,
                    Status = (int)WidgetImageStatus.Default
                };
                rs.Widgets.Add(info);
                rs.Images.Add(infoImage);
            }

            //BrandImageWidget
            if (model.BrandImageWidget != null)
            {
                rs.Widgets.Add(new Widgets()
                {
                    DecorationId = decorationId,
                    Id = Guid.NewGuid().ToString(),
                    WidgetType = (int)WidgetType.BrandImage,
                    ExtraData = JsonConvert.SerializeObject(model.BrandImageWidget)
                });
            }

            //MenuWidget
            if (model.MenuWidget != null)
            {
                rs.Widgets.Add(new Widgets()
                {
                    DecorationId = decorationId,
                    Id = Guid.NewGuid().ToString(),
                    WidgetType = (int)WidgetType.Menu,
                    ExtraData = JsonConvert.SerializeObject(model.MenuWidget)
                });
            }

            //GroupItemWidgets
            if (model.GroupItemWidget != null)
            {
                foreach (var item in model.GroupItemWidget)
                {
                    rs.Widgets.Add(new Widgets()
                    {
                        DecorationId = decorationId,
                        Id = Guid.NewGuid().ToString(),
                        WidgetType = (int)WidgetType.GroupItem,
                        ExtraData = JsonConvert.SerializeObject(item)
                    });
                }
            }

            //SliderWidgets
            if (model.SliderBannerWidget != null)
            {
                foreach (var item in model.SliderBannerWidget)
                {
                    var slider = new Widgets()
                    {
                        DecorationId = decorationId,
                        Id = Guid.NewGuid().ToString(),
                        WidgetType = (int)WidgetType.SliderBanner,
                        ExtraData = JsonConvert.SerializeObject(item)
                    };

                    foreach (var image in item.Images)
                    {
                        rs.Images.Add(new WidgetImages()
                        {
                            Id = Guid.NewGuid().ToString(),
                            WidgetId = slider.Id,
                            Image = image,
                            Status = (int)WidgetImageStatus.Default
                        });
                    }
                    rs.Widgets.Add(slider);
                }

            }

            //SingerWidgets
            if (model.SingleBannerWidget != null)
            {
                foreach (var item in model.SingleBannerWidget)
                {
                    var singel = new Widgets()
                    {
                        DecorationId = decorationId,
                        Id = Guid.NewGuid().ToString(),
                        WidgetType = (int)WidgetType.SingelBanner,
                        ExtraData = JsonConvert.SerializeObject(item)
                    };

                    rs.Images.Add(new WidgetImages()
                    {
                        Id = Guid.NewGuid().ToString(),
                        WidgetId = singel.Id,
                        Image = item.Image,
                        Status = (int)WidgetImageStatus.Default
                    });

                    rs.Widgets.Add(singel);
                }
            }

            return rs;
        }

        private async Task<Response> UpdateWidgetExtraDataAsync(string Id, string ExtraData)
        {
            if (!String.IsNullOrEmpty(Id))
            {
                var widget = await _serviceWidgets.FindAsync(Id);
                if (widget != null)
                {
                    widget.ExtraData = ExtraData;
                    _serviceWidgets.Update(widget);

                    await _unitOfWork.SaveChangesAsync();

                    return new Response { Successful = true, Message = "Update widget success" };
                }
                else
                {
                    return new Response { Successful = false, Message = "Widget not found" };
                }
            }

            return new Response { Successful = false, Message = "Please input id" };
        }
        #endregion
    }
}
