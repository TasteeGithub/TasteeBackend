
using System;
using System.Threading.Tasks;
using Tastee.Domain.Entities;
using Tastee.Shared;

namespace Tastee.Application.Interfaces
{
    public interface IBannerService : ITasteeServices<Banner>
    {
        Task<PaggingModel<Banner>> GetBannersAsync(int pageSize, int? pageIndex, string name,DateTime? fromDate,DateTime? toDate, string status);
    }
}
