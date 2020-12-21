using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Domain.Entities;
using Tastee.Shared;

namespace Tastee.Features.Brands.Queries
{
    public class GetBrandsQuery : IRequest<PaggingModel<Brand>>
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string BrandName { get; set; }

        public class GetBrandsQueryHandler : IRequestHandler<GetBrandsQuery, PaggingModel<Brand>>
        {
            private readonly IBrandService _brandService;

            public GetBrandsQueryHandler(IBrandService brandService)
            {
                _brandService = brandService;
            }

            public async Task<PaggingModel<Brand>> Handle(GetBrandsQuery request, CancellationToken cancellationToken)
            {
                return await _brandService.GetBrandsAsync(request.PageSize, request.PageIndex, request.BrandName);
            }
        }
    }
}