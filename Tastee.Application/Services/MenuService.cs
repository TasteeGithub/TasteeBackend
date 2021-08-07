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
        public async Task<Menu> GetByIdAsync(string id)
        {
            var menu = await _serviceMenus.FindAsync(id);
            return menu.Adapt<Menu>();
        }

        public async Task<PaggingModel<Menu>> GetMenusAsync(int pageSize, int? pageIndex, string name, int? status)
        {
            ExpressionStarter<Menus> searchCondition = PredicateBuilder.New<Menus>(true);

            if ((name ?? string.Empty).Length > 0)
            {
                searchCondition = searchCondition.And(x => x.Name.ToLower().Contains(name.ToLower()));
            }

            if (status != null)
            {
                searchCondition = searchCondition.And(x => x.Status == status.Value);
            }

            var listMenus = _serviceMenus.Queryable().Where(searchCondition).OrderByDescending(x => x.CreatedDate);

            var pagedListUser = await PaginatedList<Menus>.CreateAsync(listMenus, pageIndex ?? 1, pageSize);

            PaggingModel<Menu> returnResult = new PaggingModel<Menu>()
            {
                ListData = pagedListUser.Adapt<List<Menu>>(),
                TotalRows = pagedListUser.TotalRows,
            };

            return returnResult;
        }

        public async Task<Response> InsertAsync(Menu model)
        {
            if (!_serviceMenus.Queryable().Any(x => x.Name == model.Name))
            {
                Menus newMenus = model.Adapt<Menus>();
                newMenus.Id = Guid.NewGuid().ToString();
                newMenus.CreatedDate = DateTime.Now;
                newMenus.CreatedBy = model.CreatedBy;
                _serviceMenus.Insert(newMenus);
                await _unitOfWork.SaveChangesAsync();
                return new Response { Successful = true, Message = "Add menu successed" };
            }
            return new Response { Successful = false, Message = "Menu is exists" };
        }

        public async Task<Response> UpdateAsync(Menu model)
        {
            if (model.Id != null && model.Id.Length > 0)
            {
                var menu = await _serviceMenus.FindAsync(model.Id);
                if (menu != null)
                {
                    menu.Name = model.Name;
                    menu.Status = model.Status;
                    menu.Order = model.Order;
                    menu.UpdatedBy = model.UpdatedBy;
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
