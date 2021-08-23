using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tastee.Application.ViewModel;
using Tastee.Domain.Entities;
using Tastee.Infrastucture.Data.Context;
using Tastee.Shared;

namespace Tastee.Application.Interfaces
{
    public interface IMenuService : ITasteeServices<Menus>
    {
        Task<PaggingModel<Menu>> GetMenusAsync(GetMenusViewModel requestModel);
        Menus BuildMenuFromMenuModel(Menu model);
        Menu BuildMenuModelFromMenu(Menus menu);

        #region Items
        Task<MenuItem> GetItemByIdAsync(string id);
        Task<PaggingModel<MenuItem>> GetMenuItemsAsync(int pageSize, int? pageIndex, string name, int? status);
        Task<Response> InsertItemAsync(MenuItem model);
        Task<Response> UpdateItemAsync(MenuItem model);
        #endregion
    }
}
