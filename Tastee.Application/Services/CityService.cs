using LinqKit;
using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Tastee.Domain.Entities;
using Tastee.Infrastucture.Data.Context;
using Tastee.Shared;
using URF.Core.Abstractions;

namespace Tastee.Application.Interfaces
{
    public class CityService : ICityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CityService> _logger;

        private readonly IGenericService<Cities> _serviceCities;

        public CityService(
           ILogger<CityService> logger,
           IUnitOfWork unitOfWork,
           IGenericService<Cities> serviceCities
           )
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _serviceCities = serviceCities;
        }

        public async Task<List<City>> GetCitysAsync()
        {
            return _serviceCities.Queryable().Where(x => !x.IsDisabled).ToList().Adapt<List<City>>();
        }
    }
}
