using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tastee.Domain.Entities;
using Tastee.Infrastucture.Data.Context;
using Tastee.Shared;
using Tastee.Shared.Models.Topping;

namespace Tastee.Application.Interfaces
{
    public interface IToppingService : ITasteeServices<Toppings>
    {
        #region GroupTopping
        Task<PaggingModel<GroupTopping>> GetGroupToppingsAsync(GetGroupToppingViewModel requestModel);
        Task<GroupToppings> GetGroupToppingsByIdAsync(string id);
        Task<Response> InsertGroupToppingsAsync(GroupToppings model);
        Task<Response> UpdateGroupToppingsAsync(GroupToppings updateGroup);
        Task<Response> DeleteGroupToppingsAsync(string id);
        #endregion

        #region Topping
        Task<PaggingModel<Topping>> GetToppingsAsync(GetToppingViewModel requestModel);
        Task<Response> DeleteToppingsAsync(string Id);
        #endregion
    }
}
