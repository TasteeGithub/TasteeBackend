
using System.Threading.Tasks;
using Tastee.Application.ViewModel;
using Tastee.Domain.Entities;
using Tastee.Infrastucture.Data.Context;
using Tastee.Shared;

namespace Tastee.Application.Interfaces
{
    public interface IBrandService : ITasteeServices<Brands>
    {
        Task<PaggingModel<Brand>> GetBrandsAsync(GetBrandsViewModel requestModel);
        Brands BuildBrandFromBrandModel(Brand model);
        Brand BuildBrandModelFromBrand(Brands brand);
    }
}
