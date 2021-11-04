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

            var pagedListMenus = await PaginatedList<Menus>.CreateAsync(listMenus, pageIndex, pageSize);

            PaggingModel<Menu> returnResult = new PaggingModel<Menu>()
            {
                ListData = pagedListMenus.Select(x => BuildMenuModelFromMenu(x)).ToList(),
                TotalRows = pagedListMenus.TotalRows,
            };

            return returnResult;


        }

        public async Task<Response> InsertAsync(Menus newMenus)
        {
            if (!_serviceMenus.Queryable().Any(x => x.Name.ToLower() == newMenus.Name.ToLower() && x.BrandId == newMenus.BrandId))
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
                if (menu == null)
                    return new Response { Successful = false, Message = "Menu not found" };
                var brandId = String.IsNullOrEmpty(updateMenus.BrandId) ? menu.BrandId : updateMenus.BrandId;
                if (!string.IsNullOrEmpty(updateMenus.Name))
                {
                    if (_serviceMenus.Queryable().Any(x => x.Name.ToLower() == updateMenus.Name.ToLower() && x.BrandId == brandId && x.Id != updateMenus.Id))
                        return new Response { Successful = false, Message = "Menu name is exists" };
                }
                menu.Name = updateMenus.Name ?? menu.Name;
                menu.Status = updateMenus.Status;
                menu.Order = updateMenus.Order >> menu.Order;
                menu.UpdatedBy = updateMenus.UpdatedBy;
                menu.UpdatedDate = DateTime.Now;
                menu.BrandId = updateMenus.BrandId ?? menu.BrandId;
                _serviceMenus.Update(menu);
                await _unitOfWork.SaveChangesAsync();
                return new Response { Successful = true, Message = "Update menu success" };

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
            return BuildMenuItemModelFromMenuItem(menuItem);
        }

        public async Task<PaggingModel<MenuItem>> GetMenuItemsAsync(GetMenuItemsViewModel requestModel)
        {
            int pageSize = Converters.StringToInteger(requestModel.Length, Constants.DEFAULT_PAGE_SIZE).Value;
            int skip = Converters.StringToInteger(requestModel.Start).Value;
            int pageIndex = skip / pageSize + 1;

            ExpressionStarter<MenuItems> searchCondition = PredicateBuilder.New<MenuItems>(true);

            if (!String.IsNullOrEmpty(requestModel.Name))
            {
                searchCondition = searchCondition.And(x => x.Name.ToLower().Contains(requestModel.Name.ToLower()));
            }

            if (!String.IsNullOrEmpty(requestModel.MenuID))
            {
                searchCondition = searchCondition.And(x => x.MenuId == requestModel.MenuID);
            }

            if (!String.IsNullOrEmpty(requestModel.BrandId))
            {
                //Get List MenuId by BrandId
                ExpressionStarter<Menus> searchMenuCondition = PredicateBuilder.New<Menus>(true);
                searchMenuCondition = searchMenuCondition.And(x => x.BrandId == requestModel.BrandId);
                var listMenuId = _serviceMenus.Queryable().Where(searchMenuCondition).Select(x => x.Id).ToList();

                //Filter MenuItem by MenuId
                searchCondition = searchCondition.And(x => listMenuId.Contains(x.MenuId));
            }

            if (requestModel.Status != null)
            {
                searchCondition = searchCondition.And(x => x.Status == requestModel.Status.Value);
            }

            var listmenuItems = _serviceMenuItems.Queryable().Where(searchCondition).OrderByDescending(x => x.CreatedDate);

            var pagedListMenuItems = await PaginatedList<MenuItems>.CreateAsync(listmenuItems, pageIndex, pageSize);

            PaggingModel<MenuItem> returnResult = new PaggingModel<MenuItem>()
            {
                ListData = pagedListMenuItems.Select(x => BuildMenuItemModelFromMenuItem(x)).ToList(),
                TotalRows = pagedListMenuItems.TotalRows,
            };

            return returnResult;
        }

        public async Task<Response> InsertItemAsync(MenuItems newMenuItem)
        {
            newMenuItem.Id = Guid.NewGuid().ToString();
            newMenuItem.CreatedDate = DateTime.Now;
            newMenuItem.CreatedBy = newMenuItem.CreatedBy;
            _serviceMenuItems.Insert(newMenuItem);
            await _unitOfWork.SaveChangesAsync();
            return new Response { Successful = true, Message = "Add menu item successed" };
        }

        public async Task<Response> UpdateItemAsync(MenuItems updateMenuItems)
        {
            if (updateMenuItems.Id != null && updateMenuItems.Id.Length > 0)
            {
                var menuItem = await _serviceMenuItems.FindAsync(updateMenuItems.Id);
                if (menuItem != null)
                {
                    menuItem.Name = updateMenuItems.Name ?? menuItem.Name;
                    menuItem.Status = updateMenuItems.Status ?? menuItem.Status;
                    menuItem.Order = updateMenuItems.Order ?? menuItem.Order;
                    menuItem.MenuId = updateMenuItems.MenuId ?? menuItem.MenuId;
                    menuItem.Image = updateMenuItems.Image ?? menuItem.Image;
                    menuItem.Description = updateMenuItems.Description ?? menuItem.Description;
                    menuItem.ShortDescription = updateMenuItems.ShortDescription ?? menuItem.ShortDescription;
                    menuItem.Price = updateMenuItems.Price ?? menuItem.Price;
                    menuItem.LikeNumber = updateMenuItems.LikeNumber ?? menuItem.LikeNumber;
                    menuItem.UpdatedBy = updateMenuItems.UpdatedBy;
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


        public MenuItems BuildMenuItemFromMenuItemModel(MenuItem model)
        {
            var menuItem = new MenuItems()
            {
                Id = model.Id,
                Name = model.Name,
                MenuId = model.MenuId,
                Status = model.Status,
                Order = model.Order,
                Description = model.Description,
                Image = model.Image,
                LikeNumber = model.LikeNumber,
                Price = model.Price,
                SaleNumber = model.SaleNumber,
                ShortDescription = model.ShortDescription,
                CreatedDate = Converters.UnixTimeStampToDateTime(model.CreatedDate, zeroIsNull: false).Value,
                CreatedBy = model.CreatedBy,
                UpdatedDate = Converters.UnixTimeStampToDateTime(model.UpdatedDate),
                UpdatedBy = model.UpdatedBy,
            };

            return menuItem;
        }

        public MenuItem BuildMenuItemModelFromMenuItem(MenuItems menuItem)
        {
            var model = new MenuItem()
            {

                Id = menuItem.Id,
                Name = menuItem.Name,
                MenuId = menuItem.MenuId,
                Status = menuItem.Status,
                Order = menuItem.Order,
                Description = menuItem.Description,
                Image = menuItem.Image,
                LikeNumber = menuItem.LikeNumber,
                Price = menuItem.Price,
                SaleNumber = menuItem.SaleNumber,
                ShortDescription = menuItem.ShortDescription,
                CreatedDate = Converters.DateTimeToUnixTimeStamp(menuItem.CreatedDate).Value,
                CreatedBy = menuItem.CreatedBy,
                UpdatedDate = Converters.DateTimeToUnixTimeStamp(menuItem.UpdatedDate),
                UpdatedBy = menuItem.UpdatedBy,
            };
            return model;
        }
        #endregion
    }
}
