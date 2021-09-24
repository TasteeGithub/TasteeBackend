using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Application.Wrappers;
using Tastee.Infrastucture.Data.Context;
using Tastee.Shared;
using Tastee.Shared.Models.Brands.BrandDecorations;

namespace Tastee.Application.Features.Brands.BrandDecorationFeatures.Commands
{
    public class UpdateBrandDecorationCommand : IRequest<Response>
    {
        public UpdateBrandDecorationModel Model { get; set; }
        public string UserEmail { get; set; }
        public class UpdateBrandDecorationCommandHandler : IRequestHandler<UpdateBrandDecorationCommand, Response>
        {
            private readonly IFileService _fileService;
            private readonly IBrandService _brandService;
            private readonly IConfiguration _configuration;
            public UpdateBrandDecorationCommandHandler(IFileService fileService, IBrandService brandService, IConfiguration configuration)
            {
                _fileService = fileService;
                _brandService = brandService;
                _configuration = configuration;
            }
            public async Task<Response> Handle(UpdateBrandDecorationCommand request, CancellationToken cancellationToken)
            {
                var updateModel = request.Model;
                var decoration = _brandService.GetBrandDecorationByBrandId(updateModel.BrandID);
                if (decoration != null)
                    return new Response() { Successful= false, Message="Decoration not found" };

                decoration.UpdatedBy = request.UserEmail;
                decoration.WidgetsJson = JsonConvert.SerializeObject(updateModel.Widgets);

                return await _brandService.UpdateBrandDecorationAsync(decoration); ;
            }
        }
    }
}
