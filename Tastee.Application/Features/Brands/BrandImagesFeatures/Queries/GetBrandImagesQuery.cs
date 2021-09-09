using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Shared;
using Tastee.Shared.Models.Brands;
using Tastee.Domain.Entities;

namespace Tastee.Application.Features.Brands.BrandImagesFeatures.Queries
{
    public class GetBrandImagesQuery : IRequest<PaggingModel<BrandImage>>
    {
        public GetBrandImagesModel RequestModel { get; set; }
        public class GetBrandsQueryHandler : IRequestHandler<GetBrandImagesQuery, PaggingModel<BrandImage>>
        {
            private readonly IBrandService _brandService;

            public GetBrandsQueryHandler(IBrandService brandService)
            {
                _brandService = brandService;
            }

            public async Task<PaggingModel<BrandImage>> Handle(GetBrandImagesQuery request, CancellationToken cancellationToken)
            {
                return await _brandService.GetBrandImagesAsync(request.RequestModel);
            }
        }
    }
}
