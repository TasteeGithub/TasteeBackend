using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Application.Wrappers;
using Tastee.Domain.Entities;

namespace Tastee.Feature.ProductSliders.Queries
{
    public class GetProductSliderByIdQuery : IRequest<Response<ProductSlider>>
    {
        public string Id { get; set; }

        public class GetProductSliderByIdQueryHandler : IRequestHandler<GetProductSliderByIdQuery, Response<ProductSlider>>
        {
            private readonly IProductSliderService _ProductSliderService;

            public GetProductSliderByIdQueryHandler(IProductSliderService ProductSliderService)
            {
                _ProductSliderService = ProductSliderService;
            }

            public async Task<Response<ProductSlider>> Handle(GetProductSliderByIdQuery request, CancellationToken cancellationToken)
            {
                var ProductSlider = await _ProductSliderService.GetByIdAsync(request.Id);
                return new Response<ProductSlider>(ProductSlider);
            }
        }
    }
}