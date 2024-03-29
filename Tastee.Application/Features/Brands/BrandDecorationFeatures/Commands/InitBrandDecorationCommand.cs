﻿using MediatR;
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
    public class InitBrandDecorationCommand : IRequest<Response<string>>
    {
        public string BrandId { get; set; }
        public string UserEmail { get; set; }
        public class InitBrandDecorationCommandHandler : IRequestHandler<InitBrandDecorationCommand, Response<string>>
        {
            private readonly IFileService _fileService;
            private readonly IBrandService _brandService;
            private readonly IConfiguration _configuration;
            public InitBrandDecorationCommandHandler(IFileService fileService, IBrandService brandService, IConfiguration configuration)
            {
                _fileService = fileService;
                _brandService = brandService;
                _configuration = configuration;
            }
            public async Task<Response<string>> Handle(InitBrandDecorationCommand request, CancellationToken cancellationToken)
            {
                var widgetModel = new WidgetsModel();
                var decoration = _brandService.GetBrandDecorationByBrandId(request.BrandId);
                if (decoration != null)
                {
                    widgetModel = _brandService.BuildBrandDecoration(decoration);
                    return new Response<string>(JsonConvert.SerializeObject(widgetModel), null);
                }

                var brand = await _brandService.GetByIdAsync(request.BrandId);
                if (brand == null)
                    return new Response<string>("Brand not found");
                widgetModel = _brandService.BuildDefaultBrandDecoration(brand);

                await _brandService.InsertBrandDecorationAsync(widgetModel, brand.Id, request.UserEmail);
                return new Response<string>(JsonConvert.SerializeObject(widgetModel), null);
            }
        }
    }
}
