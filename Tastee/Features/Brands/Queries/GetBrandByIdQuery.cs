using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Application.Wrappers;
using Tastee.Domain.Entities;

namespace Tastee.Feature.Brands.Queries
{
    public class GetBrandByIdQuery : IRequest<Response<Brand>>
    {
        public string Id { get; set; }

        public class GetBrandByIdQueryHandler : IRequestHandler<GetBrandByIdQuery, Response<Brand>>
        {
            private readonly IBrandService _brandService;

            public GetBrandByIdQueryHandler(IBrandService brandService)
            {
                _brandService = brandService;
            }

            public async Task<Response<Brand>> Handle(GetBrandByIdQuery request, CancellationToken cancellationToken)
            {
                var brand = await _brandService.GetBrandByIdAsync(request.Id);
                return new Response<Brand>(brand);
            }
        }
    }
}