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
    public class MenuService : IMenuService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<MenuService> _logger;

        private readonly IGenericService<Menus> _serviceMenus;

        public MenuService(
           ILogger<MenuService> logger,
           IUnitOfWork unitOfWork,
           IGenericService<Menus> serviceMenus
           )
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _serviceMenus = serviceMenus;
        }

        public async Task<Menu> GetByIdAsync(string id)
        {
            var Menu = await _serviceMenus.FindAsync(id);
            return Menu.Adapt<Menu>();
        }

        public async Task<PaggingModel<Menu>> GetMenusAsync(int pageSize, int? pageIndex, string name, int status)
        {
            ExpressionStarter<Menus> searchCondition = PredicateBuilder.New<Menus>(true);

            if ((name ?? string.Empty).Length > 0)
            {
                searchCondition = searchCondition.And(x => x.Name.ToLower().Contains(name.ToLower()));
            }

            if (status > -1)
            {
                searchCondition = searchCondition.And(x => x.Status == status);
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
            if (!_serviceMenus.Queryable().Any(x => x.BrandId == model.BrandId && x.Name == model.Name))
            {
                Menus newMenus = model.Adapt<Menus>();
                newMenus.Id = Guid.NewGuid().ToString();
                //newMenus.Status = MenuStatus.Pending.ToString();
                //newMenus.CreatedDate = DateTime.Now;
                _serviceMenus.Insert(newMenus);
                await _unitOfWork.SaveChangesAsync();
                return new Response { Successful = true, Message = "Add Menu successed" };
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
                    if (!_serviceMenus.Queryable().Any(x => x.BrandId == menu.BrandId && x.Name == model.Name))
                    {
                        menu.BrandId = model.BrandId ?? menu.BrandId;
                        menu.Name = model.Name ?? menu.Name;
                        menu.Status = model.Status;
                        menu.Order = model.Order;
                        menu.UpdatedBy = model.UpdatedBy ?? menu.UpdatedBy;
                        menu.UpdatedDate = DateTime.Now;

                        _serviceMenus.Update(menu);
                        await _unitOfWork.SaveChangesAsync();

                        return new Response { Successful = true, Message = "Update Menu success" };
                    }
                    else
                    {
                        return new Response { Successful = false, Message = "Menu existsed" };
                    }
                }
                else
                {
                    return new Response { Successful = false, Message = "Menu not found" };
                }
            }
            return new Response { Successful = false, Message = "Please input id" };
        }
    }
}