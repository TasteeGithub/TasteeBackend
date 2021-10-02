
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
        Task<Response> InsertBrandDecorationAsync(BrandDecorations item);
        Task<Response> InsertDecorationImagesAsync(List<DecorationImages> items);
        Task<Response> UpdateBrandDecorationAsync(BrandDecorations item);
        BrandDecorations GetBrandDecorationByBrandId(string brandId);
        WidgetsModel BuildDefaultBrandDecoration(Brands brand);
        #endregion

    }
}
