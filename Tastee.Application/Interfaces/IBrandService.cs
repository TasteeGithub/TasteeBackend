
using System.Collections.Generic;
using System.Threading.Tasks;
using Tastee.Application.ViewModel;
using Tastee.Domain.Entities;
using Tastee.Infrastucture.Data.Context;
using Tastee.Shared;
using Tastee.Shared.Models.Brands;
using Tastee.Shared.Models.Brands.BrandDecorations;

namespace Tastee.Application.Interfaces
{
    public interface IBrandService : ITasteeServices<Brands>
    {
        #region Brand
        Task<PaggingModel<Brand>> GetBrandsAsync(GetBrandsViewModel requestModel);
        Brands BuildBrandFromModel(Brand model);
        Brand BuildModelFromBrand(Brands brand);
        Task<Response> CheckMenuItemsBelongBrand(List<string> itemIds, string brandId);
        #endregion

        #region RestaurantSpace
        Task<Response> InsertBrandImagesAsync(List<BrandImages> listRestaurantSpace);
        Task<Response> DeleteBrandImagesAsync(string id);
        Task<PaggingModel<BrandImage>> GetBrandImagesAsync(GetBrandImagesModel requestModel);
        Task<BrandImages> GetBrandImageByIdAsync(string id);
        Task<Response> UpdateBrandImageAsync(BrandImages updateImage);
        BrandImage BuildModelFromBrandImage(BrandImages brandImage);
        BrandImages BuildBrandImageFromModel(BrandImage model);
        #endregion

        #region BrandDecoration
        Task<Response> InsertBrandDecorationAsync(WidgetsModel model, string brandId, string userEmail);
        Task<Response> InsertWidgetImagesAsync(List<WidgetImages> items);
        Task<Response> UpdateBrandDecorationAsync(BrandDecorations item, WidgetsModel widgets);
        BrandDecorations GetBrandDecorationByBrandId(string brandId);
        WidgetsModel BuildDefaultBrandDecoration(Brands brand);
        WidgetsModel BuildBrandDecoration(BrandDecorations brandDecoration);
        Task<WidgetImages> GetWidgetImageByIdAsync(string id);
        Task<PaggingModel<BrandDecoration>> GetBrandDecorationAsync(GetDecorationViewModel requestModel);
        Task<PaggingModel<WidgetImage>> GetWidgetImageAsync(GetWidgetImageModel requestModel);
        Task<Response> UpdateDecorationImageAsync(UpdateDecorationImageModel updateImage);
        Task<Response> UpdateDecorationStatusAsync(UpdateDecorationStatusViewModel requestModel, string updateBy);
        Task<Response> DeleteDecorationImageAsync(string id);
        #endregion

    }
}
