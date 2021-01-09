using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Domain.Entities;

namespace Tastee.Application.Features.Areas.Queries
{
    public class GetAreasByCityIdQuery : IRequest<List<Area>>
    {
        public int CityId { get; set; }

        public class GetAreasQueryHandler : IRequestHandler<GetAreasByCityIdQuery, List<Area>>
        {
            private readonly IAreaService _areaService;

            public GetAreasQueryHandler(IAreaService areaService)
            {
                _areaService = areaService;
            }

            public Task<List<Area>> Handle(GetAreasByCityIdQuery request, CancellationToken cancellationToken)
            {
                return _areaService.GetAreasByCityIdAsync(request.CityId);
            }
        }
    }
}