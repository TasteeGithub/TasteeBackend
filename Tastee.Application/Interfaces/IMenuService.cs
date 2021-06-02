
using System;
using System.Threading.Tasks;
using Tastee.Domain.Entities;
using Tastee.Shared;

namespace Tastee.Application.Interfaces
{
    public interface IMenuService : ITasteeServices<Menu>
    {
        //Task<PaggingModel<Menu>> GetBannersAsync(int pageSize, int? pageIndex, string name,DateTime? fromDate,DateTime? toDate, string status);
        Task<PaggingModel<Menu>> GetMenusAsync(int pageSize, int? pageIndex,string name, int status);
    }
}
