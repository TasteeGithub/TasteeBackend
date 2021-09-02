using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Application.Wrappers;
using Tastee.Domain.Entities;
using Tastee.Feature.Brands.Queries;

namespace Tastee.Application.Features.Brands.BrandImagesFeatures.Queries
{
    public class GetBrandImageByIdQuery : IRequest<Response<BrandImage>>
    {
        public string Id { get; set; }

        public class GetBrandImageByIdQueryHandler : IRequestHandler<GetBrandImageByIdQuery, Response<BrandImage>>
        {
            private readonly IBrandService _brandService;

            public GetBrandImageByIdQueryHandler(IBrandService brandService)
            {
                _brandService = brandService;
            }

            public async Task<Response<BrandImage>> Handle(GetBrandImageByIdQuery request, CancellationToken cancellationToken)
            {
                var image = await _brandService.GetBrandImageByIdAsync(request.Id);
                if (image == null)
                    return new Response<BrandImage>("BrandImage not found");
                return new Response<BrandImage>(_brandService.BuildModelFromBrandImage(image));
            }
        }
    }
}