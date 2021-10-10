using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Domain.Entities;
using Tastee.Shared;
using Tastee.Shared.Models.Brands.BrandDecorations;

namespace Tastee.Application.Features.Brands.BrandDecorationFeatures.Queries
{
    public class GetDecorationImagesQuery : IRequest<PaggingModel<WidgetImage>>
    {
        public GetWidgetImageModel RequestModel { get; set; }
        public class GetDecorationImagesQueryHandler : IRequestHandler<GetDecorationImagesQuery, PaggingModel<WidgetImage>>
        {
            private readonly IBrandService _brandService;

            public GetDecorationImagesQueryHandler(IBrandService brandService)
            {
                _brandService = brandService;
            }

            public async Task<PaggingModel<WidgetImage>> Handle(GetDecorationImagesQuery request, CancellationToken cancellationToken)
            {
                return await _brandService.GetWidgetImageAsync(request.RequestModel);
            }
        }
    }
}