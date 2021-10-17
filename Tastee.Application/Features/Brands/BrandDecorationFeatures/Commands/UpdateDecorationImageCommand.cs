using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Application.Utilities;
using Tastee.Application.Wrappers;
using Tastee.Infrastucture.Data.Context;
using Tastee.Shared;
using Tastee.Shared.Models.Brands.BrandDecorations;

namespace Tastee.Application.Features.Brands.BrandDecorationFeatures.Commands
{
    public class UpdateDecorationImageCommand : IRequest<Response>
    {
        public UpdateDecorationImageModel Model { get; set; }
        public string UserEmail { get; set; }
        public class UpdateDecorationImageCommandHandler : IRequestHandler<UpdateDecorationImageCommand, Response>
        {
            private readonly IBrandService _brandService;
            public UpdateDecorationImageCommandHandler(IBrandService brandService)
            {
                _brandService = brandService;
            }
            public async Task<Response> Handle(UpdateDecorationImageCommand request, CancellationToken cancellationToken)
            {
                return await _brandService.UpdateDecorationImageAsync(request.Model);
            }
        }
    }
}
