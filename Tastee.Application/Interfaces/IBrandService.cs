
using System.Threading.Tasks;
using Tastee.Domain.Entities;
using Tastee.Shared;

namespace Tastee.Application.Interfaces
{
    public interface IBrandService
    {
        Task<PaggingModel<Brand>> GetBrandsAsync(int pageSize, int? pageIndex, string name);
        Task<Brand> GetBrandByIdAsync(string id);
        Task<Response> InsertAsync(Brand brand);
        Task<Response> UpdateAsync(Brand model);

    }
}
