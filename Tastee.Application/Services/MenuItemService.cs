using LinqKit;
using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Domain.Entities;
using Tastee.Infrastucture.Data.Context;
using Tastee.Shared;
using URF.Core.Abstractions;

namespace Tastee.Application.Services
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IGenericService<MenuItems> _serviceMenuItems;
        private readonly IGenericService<Menus> _serviceMenus;

        public MenuItemService(
           ILogger<MenuItemService> logger,
           IUnitOfWork unitOfWork,
           IGenericService<MenuItems> serviceMenuItems,
           IGenericService<Menus> serviceMenus
           )
        {
            _unitOfWork = unitOfWork;
            _serviceMenuItems = serviceMenuItems;
            _serviceMenus = serviceMenus;
        }

        public async Task<MenuItem> GetByIdAsync(string id)
        {
            var MenuItem = await _serviceMenuItems.FindAsync(id);
            return MenuItem.Adapt<MenuItem>();
        }

        public async Task<PaggingModel<MenuItem>> GetMenuItemsAsync(int pageSize, int? pageIndex, string name, int status)
        {
            ExpressionStarter<MenuItems> searchCondition = PredicateBuilder.New<MenuItems>(true);

            if ((name ?? string.Empty).Length > 0)
            {
                searchCondition = searchCondition.And(x => x.Name.ToLower().Contains(name.ToLower()));
            }

            if (status > -1)
            {
                searchCondition = searchCondition.And(x => x.Status == status);
            }

            var listMenuItems = _serviceMenuItems.Queryable().Where(searchCondition).OrderByDescending(x => x.CreatedDate);

            var pagedListUser = await PaginatedList<MenuItems>.CreateAsync(listMenuItems, pageIndex ?? 1, pageSize);

            PaggingModel<MenuItem> returnResult = new PaggingModel<MenuItem>()
            {
                ListData = pagedListUser.Adapt<List<MenuItem>>(),
                TotalRows = pagedListUser.TotalRows,
            };

            return returnResult;
        }

        public async Task<Response> InsertAsync(MenuItem model)
        {
            if (_serviceMenus.Queryable().Any(x => x.Id == model.MenuId))
            {
                if (!_serviceMenuItems.Queryable().Any(x => x.MenuId == model.MenuId && x.Name == model.Name))
                {
                    MenuItems newMenuItems = model.Adapt<MenuItems>();
                    newMenuItems.Id = Guid.NewGuid().ToString();
                    _serviceMenuItems.Insert(newMenuItems);
                    await _unitOfWork.SaveChangesAsync();
                    return new Response { Successful = true, Message = "Add MenuItem successed" };
                }
                return new Response { Successful = false, Message = "MenuItem is exists" };
            }
            else
            {
                return new Response { Successful = false, Message = "Menu is not exists" };
            }
        }

        public async Task<Response> UpdateAsync(MenuItem model)
        {
            if (model.Id != null && model.Id.Length > 0)
            {
                var menuItem = await _serviceMenuItems.FindAsync(model.Id);

                if (menuItem != null)
                {
                    if (!_serviceMenuItems.Queryable().Any(x => x.MenuId == menuItem.MenuId && x.Name == model.Name))
                    {
                        menuItem.Name = model.Name ?? menuItem.Name;
                        menuItem.MenuId = model.MenuId ?? menuItem.MenuId;
                        menuItem.Image = model.Image ?? menuItem.Image;
                        menuItem.Description = model.Description;
                        menuItem.ShortDescription = model.ShortDescription;
                        menuItem.Price = model.Price;
                        menuItem.LikeNumber = model.LikeNumber ?? menuItem.LikeNumber;
                        menuItem.SaleNumber = model.SaleNumber ?? menuItem.SaleNumber ;
                        menuItem.Status = model.Status;
                        menuItem.Order = model.Order;
                        menuItem.UpdatedBy = model.UpdatedBy ?? menuItem.UpdatedBy;
                        menuItem.UpdatedDate = DateTime.Now;

                        _serviceMenuItems.Update(menuItem);
                        await _unitOfWork.SaveChangesAsync();

                        return new Response { Successful = true, Message = "Update MenuItem success" };
                    }
                    else
                    {
                        return new Response { Successful = false, Message = "MenuItem existsed" };
                    }
                }
                else
                {
                    return new Response { Successful = false, Message = "MenuItem not found" };
                }
            }
            return new Response { Successful = false, Message = "Please input id" };
        }
    }
}