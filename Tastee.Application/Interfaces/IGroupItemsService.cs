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
        Task<PaggingModel<GroupItem>> GetGroupItemsAsync(GetGroupItemViewModel requestModel);

        List<GroupItemMapping> GetGroupItemMappingByGroupIdAsync(string GroupId);
    }
}
