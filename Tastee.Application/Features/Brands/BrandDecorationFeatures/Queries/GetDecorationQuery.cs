using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Application.ViewModel;
using Tastee.Domain.Entities;
using Tastee.Shared;

namespace Tastee.Application.Features.Brands.BrandDecorationFeatures.Queries
{
    public class GetDecorationQuery : IRequest<PaggingModel<BrandDecoration>>
    {
        public GetDecorationViewModel RequestModel { get; set; }
        public class GetDecorationQueryHandler : IRequestHandler<GetDecorationQuery, PaggingModel<BrandDecoration>>
        {
            private readonly IBrandService _brandService;

            public GetDecorationQueryHandler(IBrandService brandService)
            {
                _brandService = brandService;
            }

            public async Task<PaggingModel<BrandDecoration>> Handle(GetDecorationQuery request, CancellationToken cancellationToken)
            {
                return await _brandService.GetBrandDecorationAsync(request.RequestModel);
            }
        }
    }
}