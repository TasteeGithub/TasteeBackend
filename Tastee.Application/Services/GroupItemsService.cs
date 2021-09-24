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
using Tastee.Shared.Models.GroupItems;
using URF.Core.Abstractions;

namespace Tastee.Application.Services
{
    public class GroupItemsService : IGroupItemsService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IGenericService<GroupItems> _serviceGroupItems;
        private readonly IGenericService<GroupItemMapping> _serviceGroupItemMapping;
        public GroupItemsService(
            IUnitOfWork unitOfWork,
            IGenericService<GroupItems> serviceGroupItems,
            IGenericService<GroupItemMapping> serviceGroupItemMapping)
        {
            _unitOfWork = unitOfWork;
            _serviceGroupItems = serviceGroupItems;
            _serviceGroupItemMapping = serviceGroupItemMapping;
        }
        #region GroupItem
        public async Task<GroupItems> GetByIdAsync(string id)
        {
            var group = await _serviceGroupItems.FindAsync(id);
            return group;
        }

        public async Task<Response> InsertAsync(GroupItems model)
        {
            model.Id = Guid.NewGuid().ToString();
            model.CreatedDate = Converters.DateTimeToUnixTimeStamp(DateTime.Now).Value;
            _serviceGroupItems.Insert(model);
            await _unitOfWork.SaveChangesAsync();
            return new Response { Successful = true, Message = "Add group item successed" };
        }

        public async Task<Response> UpdateAsync(GroupItems updateGroup)
        {
            if (updateGroup.Id != null && updateGroup.Id.Length > 0)
            {
                var group = await _serviceGroupItems.FindAsync(updateGroup.Id);
                if (group != null)
                {
                    group.Name = updateGroup.Name ?? group.Name;
                    group.BrandId = updateGroup.BrandId ?? group.BrandId;
                    group.UpdatedDate = Converters.DateTimeToUnixTimeStamp(DateTime.Now).Value;
                    group.UpdatedBy = updateGroup.UpdatedBy;
                    _serviceGroupItems.Update(group);

                    await _unitOfWork.SaveChangesAsync();

                    return new Response { Successful = true, Message = "Update group item success" };
                }
                else
                {
                    return new Response { Successful = false, Message = "Group item not found" };
                }
            }

            return new Response { Successful = false, Message = "Please input id" };
        }

        public async Task<PaggingModel<GroupItem>> GetGroupItemsAsync(GetGroupItemViewModel requestModel)
        {
            ExpressionStarter<GroupItems> searchCondition = PredicateBuilder.New<GroupItems>(true);
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


            var listGroup = _serviceGroupItems.Queryable().Where(searchCondition).OrderByDescending(x => x.CreatedDate);

            var pagedListGroup = await PaginatedList<GroupItems>.CreateAsync(listGroup, pageIndex, pageSize);

            PaggingModel<GroupItem> returnResult = new PaggingModel<GroupItem>()
            {
                ListData = pagedListGroup.Adapt<List<Domain.Entities.GroupItem>>(),
                TotalRows = pagedListGroup.TotalRows,
            };
            return returnResult;
        }

        public GroupItemDetail BuildGroupItemDetail(GroupItems group)
        {
            GroupItemDetail detail = group.Adapt<GroupItemDetail>();
            detail.MenuItemIds = GetGroupItemMappingByGroupIdAsync(group.Id).Select(x=>x.ItemId).ToList();
            return detail;
        }
        #endregion

        #region GroupItemsMapping
        public List<GroupItemMapping> GetGroupItemMappingByGroupIdAsync(string GroupId)
        {
            ExpressionStarter<GroupItemMapping> searchCondition = PredicateBuilder.New<GroupItemMapping>(true);
            searchCondition = searchCondition.And(x => x.GroupId == GroupId);
            var listItems = _serviceGroupItemMapping.Queryable().Where(searchCondition).OrderByDescending(x => x.CreatedDate).ToList();
            return listItems;
        }

        public async Task<Response> InsertGroupItemMappingAsync(List<string> itemIds, string groupId, string createdBy)
        {
            var existItemIds = _serviceGroupItemMapping.Queryable().Where(x => itemIds.Contains(x.ItemId) && x.GroupId == groupId).Select(x => x.ItemId).ToList();
            var listItemIds = itemIds.Except(existItemIds).ToList();
            foreach (var itemId in listItemIds)
            {
                _serviceGroupItemMapping.Insert(new GroupItemMapping
                {
                    CreatedBy = createdBy,
                    CreatedDate = Converters.DateTimeToUnixTimeStamp(DateTime.Now).Value,
                    GroupId = groupId,
                    ItemId = itemId,
                });
            }
            await _unitOfWork.SaveChangesAsync();
            return new Response { Successful = true, Message = "Add items successed" };
        }


        public async Task<Response> DeleteGroupItemMapping(List<string> itemIds, string groupId)
        {
            var items = _serviceGroupItemMapping.Queryable().Where(x => itemIds.Contains(x.ItemId) && x.GroupId == groupId).ToList();
            foreach (var item in items)
            {
                _serviceGroupItemMapping.Delete(item);
            }
            await _unitOfWork.SaveChangesAsync();
            return new Response { Successful = true, Message = "Delete successed" };
        }

        #endregion
    }
}
