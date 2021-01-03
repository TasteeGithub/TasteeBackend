
using System.Threading.Tasks;
using Tastee.Domain.Entities;
using Tastee.Shared;

namespace Tastee.Application.Interfaces
{
    public interface IBrandService : ITasteeServices<Brand>
    {
        Task<PaggingModel<Brand>> GetBrandsAsync(int pageSize, int? pageIndex, string name);
    }
}
