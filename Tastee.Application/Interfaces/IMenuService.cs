using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tastee.Domain.Entities;
using Tastee.Shared;

namespace Tastee.Application.Interfaces
{
    public interface IMenuService : ITasteeServices<Menu>
    {
        Task<PaggingModel<Menu>> GetMenusAsync(int pageSize, int? pageIndex, string name, int? status);

        #region Items
        Task<MenuItem> GetItemByIdAsync(string id);
        Task<PaggingModel<MenuItem>> GetMenuItemsAsync(int pageSize, int? pageIndex, string name, int? status);
        Task<Response> InsertItemAsync(MenuItem model);
        Task<Response> UpdateItemAsync(MenuItem model);
        #endregion
    }
}
