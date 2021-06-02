using Mapster;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Domain.Entities;
using Tastee.Infrastucture.Data.Context;

namespace Tastee.Application.Services
{
    public class AreaService : IAreaService
    {
        private readonly ILogger<AreaService> _logger;

        private readonly IGenericService<Areas> _serviceAreas;

        public AreaService(
           ILogger<AreaService> logger,
           IGenericService<Areas> serviceAreas
           )
        {
            _logger = logger;
            _serviceAreas = serviceAreas;
        }

        public async Task<List<Area>> GetAreasAsync() => _serviceAreas.Queryable().Where(x => !x.IsDisabled).ToList().Adapt<List<Area>>();

        public async Task<List<Area>> GetAreasByCityIdAsync(int cityId) => _serviceAreas.Queryable().Where(x => x.CityId == cityId).ToList().Adapt<List<Area>>();

        public async Task<Area> GetByIdAsync(int id)
        {
            var area = await _serviceAreas.FindAsync(id);
            return area.Adapt<Area>();
        }
    }
}