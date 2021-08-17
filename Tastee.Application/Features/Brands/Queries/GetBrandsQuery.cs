using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Application.ViewModel;
using Tastee.Domain.Entities;
using Tastee.Shared;

namespace Tastee.Features.Brands.Queries
{

    public class GetBrandsQuery : IRequest<PaggingModel<Brand>>
    {
        public GetBrandsViewModel RequestModel { get; set; }
        public class GetBrandsQueryHandler : IRequestHandler<GetBrandsQuery, PaggingModel<Brand>>
        {
            private readonly IBrandService _brandService;

            public GetBrandsQueryHandler(IBrandService brandService)
            {
                _brandService = brandService;
            }

            public async Task<PaggingModel<Brand>> Handle(GetBrandsQuery request, CancellationToken cancellationToken)
            {
                return await _brandService.GetBrandsAsync(request.RequestModel);
            }
        }
    }
}