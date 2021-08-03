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
        private readonly ILogger<BannerService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        private readonly IGenericService<Menus> _serviceMenus;

        public MenuService(
           ILogger<BannerService> logger,
           IUnitOfWork unitOfWork,
           IGenericService<Menus> serviceMenus)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;

            _serviceMenus = serviceMenus;
        }

        public async Task<Menu> GetByIdAsync(string id)
        {
            var banner = await _serviceMenus.FindAsync(id);
            return banner.Adapt<Menu>();
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
                newMenus.Status = model.Status;
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

    }
}
