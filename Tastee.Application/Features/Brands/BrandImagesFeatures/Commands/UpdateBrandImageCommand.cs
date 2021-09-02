using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Domain.Entities;
using Tastee.Shared;
using Tastee.Shared.Models.Brands;

namespace Tastee.Application.Features.Brands.BrandImagesFeatures.Commands
{
    public class UpdateBrandImageCommand : IRequest<Response>
    {
        public UpdateBrandImageModel Model;
        public string UpdatedBy;
    }

    public class UpdateBrandCommandHandler : IRequestHandler<UpdateBrandImageCommand, Response>
    {
        private readonly IBrandService _brandService;

        public UpdateBrandCommandHandler(IBrandService brandService)
        {
            _brandService = brandService;
        }

        public async Task<Response> Handle(UpdateBrandImageCommand request, CancellationToken cancellationToken)
        {
            var model = request.Model.Adapt<BrandImage>();
            model.UpdatedBy = request.UpdatedBy;
            var brand = _brandService.BuildBrandImageFromModel(model);
            return await _brandService.UpdateBrandImageAsync(brand);
        }
    }
}