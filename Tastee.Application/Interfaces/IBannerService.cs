
using System;
using System.Threading.Tasks;
using Tastee.Domain.Entities;
using Tastee.Shared;

namespace Tastee.Application.Interfaces
{
    public interface IBannerService : ITasteeServices<Banner>
    {
        Task<PaggingModel<BannerSimple>> GetBannersAsync(int pageSize, int? pageIndex, string name, long? fromDate,long? toDate, string status, int? type, bool? isDisplay);
        Task<Response> DeleteBannerAsync(string Id);
        Task<Response> UpdateImageAsync(string id, string url);
    }
}
