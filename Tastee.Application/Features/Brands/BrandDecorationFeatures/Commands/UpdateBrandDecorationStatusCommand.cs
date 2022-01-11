using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Application.ViewModel;
using Tastee.Application.Wrappers;
using Tastee.Shared;

namespace Tastee.Application.Features.Brands.BrandDecorationFeatures.Commands
{

    public class UpdateBrandDecorationStatusCommand : IRequest<Response>
    {
        public UpdateDecorationStatusViewModel Model { get; set; }
        public string UserEmail { get; set; }
        public class UpdateBrandDecorationStatusCommandHandler : IRequestHandler<UpdateBrandDecorationStatusCommand, Response>
        {
            private readonly IBrandService _brandService;
            public UpdateBrandDecorationStatusCommandHandler(IBrandService brandService)
            {
                _brandService = brandService;
            }
            public async Task<Response> Handle(UpdateBrandDecorationStatusCommand request, CancellationToken cancellationToken)
            {
                return await _brandService.UpdateDecorationStatusAsync(request.Model, request.UserEmail);
            }
        }
    }
}
