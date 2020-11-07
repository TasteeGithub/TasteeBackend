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
    public class AreaService : IAreaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AreaService> _logger;

        private readonly IGenericService<Areas> _serviceAreas;

        public AreaService(
           ILogger<AreaService> logger,
           IUnitOfWork unitOfWork,
           IGenericService<Areas> serviceAreas
           )
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _serviceAreas = serviceAreas;
        }

        //public Task<List<Area>> GetAreasAsync()
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<List<Area>> GetAreasAsync()
        {
            return _serviceAreas.Queryable().Where(x => !x.IsDisabled).ToList().Adapt<List<Area>>();
        }
    }
}
