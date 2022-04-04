using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Application.ViewModel;
using Tastee.Domain.Entities;
using Tastee.Shared;
using Tastee.Shared.Models.Brands;

namespace Tastee.Features.Brands.Queries
{

    public class GetSuggestBrandsQuery : IRequest<PaggingModel<SuggestBrandModel>>
    {
        public GetSuggestBrandsViewModel RequestModel { get; set; }
        public class GetSuggestBrandsQueryHandler : IRequestHandler<GetSuggestBrandsQuery, PaggingModel<SuggestBrandModel>>
        {
            private readonly IBrandService _brandService;

            public GetSuggestBrandsQueryHandler(IBrandService brandService)
            {
                _brandService = brandService;
            }

            public async Task<PaggingModel<SuggestBrandModel>> Handle(GetSuggestBrandsQuery request, CancellationToken cancellationToken)
            {
                return await _brandService.GetSuggestBrandsAsync(request.RequestModel);
            }
        }
    }
}