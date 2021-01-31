
using System;
using System.Threading.Tasks;
using Tastee.Domain.Entities;
using Tastee.Shared;

namespace Tastee.Application.Interfaces
{
    public interface IProductSliderService : ITasteeServices<ProductSlider>
    {
        Task<PaggingModel<ProductSlider>> GetProductSlidersAsync(int pageSize, int? pageIndex, string brandId,DateTime? fromDate,DateTime? toDate, string status);
    }
}
