
using System;
using System.Threading.Tasks;
using Tastee.Domain.Entities;
using Tastee.Shared;

namespace Tastee.Application.Interfaces
{
    public interface IMenuItemService : ITasteeServices<MenuItem>
    {
        Task<PaggingModel<MenuItem>> GetMenuItemsAsync(int pageSize, int? pageIndex,string name, int status);
    }
}
