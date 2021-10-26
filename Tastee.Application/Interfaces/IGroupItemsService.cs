using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tastee.Domain.Entities;
using Tastee.Infrastucture.Data.Context;
using Tastee.Shared;
using Tastee.Shared.Models.GroupItems;

namespace Tastee.Application.Interfaces
{
    public interface IGroupItemsService : ITasteeServices<GroupItems>
    {
        #region GroupItem
        Task<PaggingModel<GroupItem>> GetGroupItemsAsync(GetGroupItemViewModel requestModel);
        GroupItemDetail BuildGroupItemDetail(GroupItems group);
        #endregion

        #region GroupItemsMapping
        Task<PaggingModel<GroupItemMappingInfo>> GetGroupItemMappingAsync(GetGroupItemMappingViewModel requestModel);
        List<GroupItemMapping> GetGroupItemMappingByGroupIdAsync(string GroupId);
        Task<Response> InsertGroupItemMappingAsync(List<string> itemIds, string groupId, string createdBy);
        Task<Response> DeleteGroupItemMapping(List<string> itemIds, string groupId);
        #endregion

    }
}
