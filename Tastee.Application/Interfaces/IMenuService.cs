using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tastee.Domain.Entities;
using Tastee.Shared;

namespace Tastee.Application.Interfaces
{
    public interface IMenuService : ITasteeServices<Menu>
    {
        Task<PaggingModel<Menu>> GetMenusAsync(int pageSize, int? pageIndex, string name, int? status);
    }
}
