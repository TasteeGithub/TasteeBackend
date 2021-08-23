using LinqKit;
using Mapster;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Application.Utilities;
using Tastee.Application.ViewModel;
using Tastee.Domain.Entities;
using Tastee.Infrastucture.Data.Context;
using Tastee.Shared;
using URF.Core.Abstractions;

namespace Tastee.Application.Services
{
    public class MenuService : IMenuService
    {
        private readonly ILogger<MenuService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        private readonly IGenericService<Menus> _serviceMenus;
        private readonly IGenericService<MenuItems> _serviceMenuItems;

        public MenuService(
           ILogger<MenuService> logger,
           IUnitOfWork unitOfWork,
           IGenericService<Menus> serviceMenus,
           IGenericService<MenuItems> serviceMenuItems)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;

            _serviceMenus = serviceMenus;
            _serviceMenuItems = serviceMenuItems;
        }


        #region Menus
        public async Task<Menus> GetByIdAsync(string id)
        {
            var menu = await _serviceMenus.FindAsync(id);
            return menu;
        }

        public async Task<PaggingModel<Menu>> GetMenusAsync(GetMenusViewModel requestModel)
        {
            ExpressionStarter<Menus> searchCondition = PredicateBuilder.New<Menus>(true);
            int pageSize = Converters.StringToInteger(requestModel.Length, Constants.DEFAULT_PAGE_SIZE).Value;
            int skip = Converters.StringToInteger(requestModel.Start).Value;
            int pageIndex = skip / pageSize + 1;

            if (!string.IsNullOrEmpty(requestModel.Name))
            {
                searchCondition = searchCondition.And(x => x.Name.ToLower().Contains(requestModel.Name.ToLower()));
            }

            if (requestModel.Status != null)
            {
                searchCondition = searchCondition.And(x => x.Status == requestModel.Status);
            }

            if (!string.IsNullOrEmpty(requestModel.BrandId))
            {
                searchCondition = searchCondition.And(x => x.BrandId == requestModel.BrandId);
            }

            var listMenus = _serviceMenus.Queryable().Where(searchCondition).OrderByDescending(x => x.CreatedDate);

            var pagedListUser = await PaginatedList<Menus>.CreateAsync(listMenus, pageIndex, pageSize);

            PaggingModel<Menu> returnResult = new PaggingModel<Menu>()
            {
                ListData = pagedListUser.Select(x=>BuildMenuModelFromMenu(x)).ToList(),
                TotalRows = pagedListUser.TotalRows,
            };

            return returnResult;


        }

        public async Task<Response> InsertAsync(Menus newMenus)
        {
            if (!_serviceMenus.Queryable().Any(x => x.Name == newMenus.Name))
            {
                newMenus.Id = Guid.NewGuid().ToString();
                newMenus.CreatedDate = DateTime.Now;
                _serviceMenus.Insert(newMenus);
                await _unitOfWork.SaveChangesAsync();
                return new Response { Successful = true, Message = "Add menu successed" };
            }
            return new Response { Successful = false, Message = "Menu is exists" };
        }

        public async Task<Response> UpdateAsync(Menus updateMenus)
        {
            if (updateMenus.Id != null && updateMenus.Id.Length > 0)
            {
                var menu = await _serviceMenus.FindAsync(updateMenus.Id);
                if (menu != null)
                {
                    menu.Name = updateMenus.Name;
                    menu.Status = updateMenus.Status;
                    menu.Order = updateMenus.Order;
                    menu.UpdatedBy = updateMenus.UpdatedBy;
                    menu.UpdatedDate = DateTime.Now;
                    _serviceMenus.Update(menu);
                    await _unitOfWork.SaveChangesAsync();

                    return new Response { Successful = true, Message = "Update menu success" };
                }
                else
                {
                    return new Response { Successful = false, Message = "Menu not found" };
                }
            }
            return new Response { Successful = false, Message = "Please input id" };
        }

        public Menus BuildMenuFromMenuModel(Menu model)
        {
            var menu = new Menus()
            {
                Id = model.Id,
                Name = model.Name,
                BrandId = model.BrandId,
                Status = model.Status,
                Order = model.Order,
                CreatedDate = Converters.UnixTimeStampToDateTime(model.CreatedDate, zeroIsNull: false).Value,
                CreatedBy = model.CreatedBy,
                UpdatedDate = Converters.UnixTimeStampToDateTime(model.UpdatedDate),
                UpdatedBy = model.UpdatedBy,
            };

            return menu;
        }

        public Menu BuildMenuModelFromMenu(Menus menu)
        {
            var model = new Menu()
            {

                Id = menu.Id,
                Name = menu.Name,
                BrandId = menu.BrandId,
                Status = menu.Status,
                Order = menu.Order,
                CreatedDate = Converters.DateTimeToUnixTimeStamp(menu.CreatedDate).Value,
                CreatedBy = menu.CreatedBy,
                UpdatedDate = Converters.DateTimeToUnixTimeStamp(menu.UpdatedDate),
                UpdatedBy = menu.UpdatedBy,
            };
            return model;
        }
        #endregion

        #region Items
        public async Task<MenuItem> GetItemByIdAsync(string id)
        {
            var menuItem = await _serviceMenuItems.FindAsync(id);
            return menuItem.Adapt<MenuItem>();
        }

        public async Task<PaggingModel<MenuItem>> GetMenuItemsAsync(int pageSize, int? pageIndex, string name, int? status)
        {
            ExpressionStarter<MenuItems> searchCondition = PredicateBuilder.New<MenuItems>(true);

            if ((name ?? string.Empty).Length > 0)
            {
                searchCondition = searchCondition.And(x => x.Name.ToLower().Contains(name.ToLower()));
            }

            if (status != null)
            {
                searchCondition = searchCondition.And(x => x.Status == status.Value);
            }

            var listmenuItems = _serviceMenuItems.Queryable().Where(searchCondition).OrderByDescending(x => x.CreatedDate);

            var pagedListUser = await PaginatedList<MenuItems>.CreateAsync(listmenuItems, pageIndex ?? 1, pageSize);

            PaggingModel<MenuItem> returnResult = new PaggingModel<MenuItem>()
            {
                ListData = pagedListUser.Adapt<List<MenuItem>>(),
                TotalRows = pagedListUser.TotalRows,
            };

            return returnResult;
        }

        public async Task<Response> InsertItemAsync(MenuItem model)
        {
            if (!_serviceMenus.Queryable().Any(x => x.Name == model.Name))
            {
                MenuItems newmenuItems = model.Adapt<MenuItems>();
                newmenuItems.Id = Guid.NewGuid().ToString();
                newmenuItems.CreatedDate = DateTime.Now;
                newmenuItems.CreatedBy = model.CreatedBy;
                _serviceMenuItems.Insert(newmenuItems);
                await _unitOfWork.SaveChangesAsync();
                return new Response { Successful = true, Message = "Add menu item successed" };
            }
            return new Response { Successful = false, Message = "Menu item is exists" };
        }

        public async Task<Response> UpdateItemAsync(MenuItem model)
        {
            if (model.Id != null && model.Id.Length > 0)
            {
                var menuItem = await _serviceMenuItems.FindAsync(model.Id);
                if (menuItem != null)
                {
                    menuItem.Name = model.Name;
                    menuItem.Status = model.Status;
                    menuItem.Order = model.Order;
                    menuItem.MenuId = model.MenuId;
                    menuItem.Image = model.Image;
                    menuItem.Description = model.Description;
                    menuItem.ShortDescription = model.ShortDescription;
                    menuItem.Price = model.Price;
                    menuItem.LikeNumber = model.LikeNumber;
                    menuItem.UpdatedBy = model.UpdatedBy;
                    menuItem.UpdatedDate = DateTime.Now;
                    _serviceMenuItems.Update(menuItem);
                    await _unitOfWork.SaveChangesAsync();

                    return new Response { Successful = true, Message = "Update menu item success" };
                }
                else
                {
                    return new Response { Successful = false, Message = "Menu item not found" };
                }
            }
            return new Response { Successful = false, Message = "Please input id" };
        }
        #endregion
    }
}
