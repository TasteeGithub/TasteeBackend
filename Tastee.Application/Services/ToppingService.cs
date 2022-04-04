using LinqKit;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Application.Utilities;
using Tastee.Domain.Entities;
using Tastee.Infrastucture.Data.Context;
using Tastee.Shared;
using Tastee.Shared.Models.Topping;
using URF.Core.Abstractions;

namespace Tastee.Application.Services
{
    public class ToppingService : IToppingService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IGenericService<Toppings> _serviceToppings;
        private readonly IGenericService<GroupToppings> _serviceGroupToppings;
        public ToppingService(
            IUnitOfWork unitOfWork,
            IGenericService<Toppings> serviceToppings,
            IGenericService<GroupToppings> serviceGroupToppings)
        {
            _unitOfWork = unitOfWork;
            _serviceToppings = serviceToppings;
            _serviceGroupToppings = serviceGroupToppings;
        }

        #region GroupToppings
        public async Task<GroupToppings> GetGroupToppingsByIdAsync(string id)
        {
            var group = await _serviceGroupToppings.FindAsync(id);
            return group;
        }

        public async Task<Response> InsertGroupToppingsAsync(GroupToppings model)
        {
            model.Id = Guid.NewGuid().ToString();
            model.CreatedDate = Converters.DateTimeToUnixTimeStamp(DateTime.Now).Value;
            _serviceGroupToppings.Insert(model);
            await _unitOfWork.SaveChangesAsync();
            return new Response { Successful = true, Message = "Add group topping successed" };
        }

        public async Task<Response> UpdateGroupToppingsAsync(GroupToppings updateGroup)
        {
            if (updateGroup.Id != null && updateGroup.Id.Length > 0)
            {
                var group = await _serviceGroupToppings.FindAsync(updateGroup.Id);
                if (group != null)
                {
                    group.Name = updateGroup.Name;
                    group.BrandId = updateGroup.BrandId;
                    group.MenuItemId = updateGroup.MenuItemId;
                    group.Min = updateGroup.Min;
                    group.Max = updateGroup.Max;
                    group.Status = updateGroup.Status;
                    group.IsRequired = updateGroup.IsRequired;
                    group.DisplayOrder = updateGroup.DisplayOrder;
                    group.UpdatedDate = Converters.DateTimeToUnixTimeStamp(DateTime.Now).Value;
                    group.UpdatedBy = updateGroup.UpdatedBy;
                    _serviceGroupToppings.Update(group);

                    await _unitOfWork.SaveChangesAsync();

                    return new Response { Successful = true, Message = "Update group toppings success" };
                }
                else
                {
                    return new Response { Successful = false, Message = "Group topping not found" };
                }
            }

            return new Response { Successful = false, Message = "Please input id" };
        }

        public async Task<PaggingModel<GroupTopping>> GetGroupToppingsAsync(GetGroupToppingViewModel requestModel)
        {
            ExpressionStarter<GroupToppings> searchCondition = PredicateBuilder.New<GroupToppings>(true);
            int pageSize = Converters.StringToInteger(requestModel.Length, Constants.DEFAULT_PAGE_SIZE).Value;
            int skip = Converters.StringToInteger(requestModel.Start).Value;
            int pageIndex = skip / pageSize + 1;

            if (!string.IsNullOrEmpty(requestModel.Name))
            {
                searchCondition = searchCondition.And(x => x.Name.ToLower().Contains(requestModel.Name.ToLower()));
            }

            if (!string.IsNullOrEmpty(requestModel.BrandId))
            {
                searchCondition = searchCondition.And(x => x.BrandId.ToLower() == requestModel.BrandId.ToLower());
            }

            if (!string.IsNullOrEmpty(requestModel.MenuItemId))
            {
                searchCondition = searchCondition.And(x => x.MenuItemId.ToLower() == requestModel.MenuItemId.ToLower());
            }

            if (requestModel.Status != null)
            {
                searchCondition = searchCondition.And(x => x.Status == (int)requestModel.Status.Value);
            }


            var listGroup = _serviceGroupToppings.Queryable().Where(searchCondition).OrderByDescending(x => x.CreatedDate);

            var pagedListGroup = await PaginatedList<GroupToppings>.CreateAsync(listGroup, pageIndex, pageSize);

            PaggingModel<GroupTopping> returnResult = new PaggingModel<GroupTopping>()
            {
                ListData = pagedListGroup.Adapt<List<GroupTopping>>(),
                TotalRows = pagedListGroup.TotalRows,
            };
            return returnResult;
        }

        public async Task<Response> DeleteGroupToppingsAsync(string Id)
        {
            var topping = _serviceToppings.Queryable().Where(x => x.GroupToppingId == Id).FirstOrDefault();
            if(topping != null)
            {
                return new Response { Successful = false, Message = "There are topping belong group" };
            }
            var group = await GetGroupToppingsByIdAsync(Id);
            if (group == null)
            {
                return new Response { Successful = true, Message = "Delete group topping successed" };
            }
            _serviceGroupToppings.Delete(group);
            await _unitOfWork.SaveChangesAsync();
            return new Response { Successful = true, Message = "Delete group topping successed" };
        }

        #endregion

        #region Toppings
        public async Task<Toppings> GetByIdAsync(string id)
        {
            var group = await _serviceToppings.FindAsync(id);
            return group;
        }

        public async Task<Response> InsertAsync(Toppings model)
        {
            model.Id = Guid.NewGuid().ToString();
            model.CreatedDate = Converters.DateTimeToUnixTimeStamp(DateTime.Now).Value;
            _serviceToppings.Insert(model);
            await _unitOfWork.SaveChangesAsync();
            return new Response { Successful = true, Message = "Add topping successed" };
        }

        public async Task<Response> UpdateAsync(Toppings updateTopping)
        {
            if (updateTopping.Id != null && updateTopping.Id.Length > 0)
            {
                var group = await _serviceToppings.FindAsync(updateTopping.Id);
                if (group != null)
                {
                    group.Name = updateTopping.Name;
                    group.GroupToppingId = updateTopping.GroupToppingId;
                    group.Status = updateTopping.Status;
                    group.OrderBy = updateTopping.OrderBy;
                    group.IsRequired = updateTopping.IsRequired;
                    group.Price = updateTopping.Price;
                    group.Max = updateTopping.Max;
                    group.Min = updateTopping.Min;
                    group.IsSelected = updateTopping.IsSelected;
                    group.Note = updateTopping.Note;
                    group.UpdatedDate = Converters.DateTimeToUnixTimeStamp(DateTime.Now).Value;
                    group.UpdatedBy = updateTopping.UpdatedBy;
                    _serviceToppings.Update(group);

                    await _unitOfWork.SaveChangesAsync();

                    return new Response { Successful = true, Message = "Update topping success" };
                }
                else
                {
                    return new Response { Successful = false, Message = "Topping not found" };
                }
            }

            return new Response { Successful = false, Message = "Please input id" };
        }

        public async Task<PaggingModel<Topping>> GetToppingsAsync(GetToppingViewModel requestModel)
        {
            ExpressionStarter<Toppings> searchCondition = PredicateBuilder.New<Toppings>(true);
            int pageSize = Converters.StringToInteger(requestModel.Length, Constants.DEFAULT_PAGE_SIZE).Value;
            int skip = Converters.StringToInteger(requestModel.Start).Value;
            int pageIndex = skip / pageSize + 1;

            if (!string.IsNullOrEmpty(requestModel.Name))
            {
                searchCondition = searchCondition.And(x => x.Name.ToLower().Contains(requestModel.Name.ToLower()));
            }

            if (!string.IsNullOrEmpty(requestModel.GroupToppingId))
            {
                searchCondition = searchCondition.And(x => x.GroupToppingId.ToLower().Contains(requestModel.GroupToppingId.ToLower()));
            }

            if (requestModel.Status != null)
            {
                searchCondition = searchCondition.And(x => x.Status == (int)requestModel.Status.Value);
            }

            var toppings = _serviceToppings.Queryable().Where(searchCondition).OrderByDescending(x => x.CreatedDate);

            var pagedListGroup = await PaginatedList<Toppings>.CreateAsync(toppings, pageIndex, pageSize);

            PaggingModel<Topping> returnResult = new PaggingModel<Topping>()
            {
                ListData = pagedListGroup.Adapt<List<Domain.Entities.Topping>>(),
                TotalRows = pagedListGroup.TotalRows,
            };
            return returnResult;
        }

        public async Task<Response> DeleteToppingsAsync(string Id)
        {
            var topping = _serviceToppings.Queryable().Where(x => x.Id == Id).FirstOrDefault();
            if (topping == null)
            {
                return new Response { Successful = true, Message = "Delete topping successed" };
            }
            _serviceToppings.Delete(topping);
            await _unitOfWork.SaveChangesAsync();
            return new Response { Successful = true, Message = "Delete topping successed" };
        }
        #endregion
    }
}
