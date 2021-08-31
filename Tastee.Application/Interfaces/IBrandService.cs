
using System.Collections.Generic;
using System.Threading.Tasks;
using Tastee.Application.ViewModel;
using Tastee.Domain.Entities;
using Tastee.Infrastucture.Data.Context;
using Tastee.Shared;
using Tastee.Shared.Models.Brands;

namespace Tastee.Application.Interfaces
{
    public interface IBrandService : ITasteeServices<Brands>
    {
        #region Brand
        Task<PaggingModel<Brand>> GetBrandsAsync(GetBrandsViewModel requestModel);
        Brands BuildBrandFromBrandModel(Brand model);
        Brand BuildBrandModelFromBrand(Brands brand);
        #endregion

        #region RestaurantSpace
        Task<Response> InsertRangeRestaurantSpaceAsync(List<BrandImages> listRestaurantSpace);
        Task<PaggingModel<BrandImage>> GetBrandImagesAsync(GetBrandImagesModel requestModel);
        #endregion

    }
}
