using Mapster;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Application.Wrappers;
using Tastee.Domain.Entities;

namespace Tastee.Feature.Areas.Queries
{
    public class GetAreaByIdQuery : IRequest<Area>
    {
        public int Id { get; set; }

        public class GetAreaByIdQueryHandler : IRequestHandler<GetAreaByIdQuery, Area>
        {
            private readonly IAreaService _areaService;

            public GetAreaByIdQueryHandler(IAreaService areaService)
            {
                _areaService = areaService;
            }

            public async Task<Area> Handle(GetAreaByIdQuery request, CancellationToken cancellationToken)
            {
                var area = await _areaService.GetByIdAsync(request.Id);
                return area.Adapt<Area>();
            }
        }
    }
}