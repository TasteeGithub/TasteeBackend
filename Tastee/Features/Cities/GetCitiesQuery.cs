using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Domain.Entities;

namespace Tastee.Features.Cities.Queries
{
    public class GetCitiesQuery : IRequest<List<City>>
    {
        public class GetCitiesQueryHandler : IRequestHandler<GetCitiesQuery,List<City>>
        {
            private readonly ICityService _citiesService;

            public GetCitiesQueryHandler(ICityService citieservice)
            {
                _citiesService = citieservice;
            }

            public async Task<List<City>> Handle(GetCitiesQuery request, CancellationToken cancellationToken)
            {
                return await _citiesService.GetCitysAsync();
            }
        }
    }
}