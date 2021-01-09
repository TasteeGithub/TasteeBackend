
using System.Collections.Generic;
using System.Threading.Tasks;
using Tastee.Domain.Entities;

namespace Tastee.Application.Interfaces
{
    public interface IAreaService
    {
        Task<List<Area>> GetAreasAsync();
        Task<Area> GetByIdAsync(int id);
        Task<List<Area>> GetAreasByCityIdAsync(int cityId);
    }
}
