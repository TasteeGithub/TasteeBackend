using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Domain.Entities;

namespace Tastee.Application.Features.Areas.Queries
{
    public class GetAreasQuery: IRequest<List<Area>>
    {

        public class GetAreasQueryHandler : IRequestHandler<GetAreasQuery, List<Area>>
        {
            private readonly IAreaService _areaService;
            public GetAreasQueryHandler(IAreaService areaService )
            {
                _areaService = areaService;
            }

            public Task<List<Area>> Handle(GetAreasQuery request, CancellationToken cancellationToken)
            {
                return _areaService.GetAreasAsync();
            }
        }
    }
}
