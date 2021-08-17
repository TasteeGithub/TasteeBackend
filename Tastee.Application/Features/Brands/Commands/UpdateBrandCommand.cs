using Mapster;
using MediatR;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Application.ViewModel;
using Tastee.Domain.Entities;
using Tastee.Shared;

namespace Tastee.Features.Brands.Commands
{
    public class UpdateBrandCommand : IRequest<Response>
    {
        public Brand BrandModel;
    }

    public class UpdateBrandCommandHandler : IRequestHandler<UpdateBrandCommand, Response>
    {
        private readonly IBrandService _brandService;

        public UpdateBrandCommandHandler(IBrandService brandService)
        {
            _brandService = brandService;
        }

        public async Task<Response> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
        {
            var brand = _brandService.BuildBrandFromBrandModel(request.BrandModel);
            return await _brandService.UpdateAsync(brand);
        }
    }
}