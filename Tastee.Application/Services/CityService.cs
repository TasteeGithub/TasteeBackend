using Mapster;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Domain.Entities;
using Tastee.Infrastucture.Data.Context;
using URF.Core.Abstractions;

namespace Tastee.Application.Services
{
    public class CityService : ICityService
    {
        private readonly ILogger<CityService> _logger;

        private readonly IGenericService<Cities> _serviceCities;

        public CityService(
           ILogger<CityService> logger,
           IGenericService<Cities> serviceCities
           )
        {
            _logger = logger;
            _serviceCities = serviceCities;
        }

        public async Task<List<City>> GetCitysAsync()
        {
            return _serviceCities.Queryable().Where(x => !x.IsDisabled).ToList().Adapt<List<City>>();
        }
    }
}