
using System.Collections.Generic;
using System.Threading.Tasks;
using Tastee.Domain.Entities;

namespace Tastee.Application.Interfaces
{
    public interface IAreaService
    {
        Task<List<Area>> GetAreasAsync();
    }
}
