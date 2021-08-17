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
    public class CreateBrandCommand : IRequest<Response>
    {
        public Brand BrandModel;
    }

    public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, Response>
    {
        private readonly IBrandService _brandService;
        private readonly IFileService _fileService;

        public CreateBrandCommandHandler(IBrandService brandService, IFileService fileService)
        {
            _brandService = brandService;
            _fileService = fileService;
        }

        public async Task<Response> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
            var brand = _brandService.BuildBrandFromBrandModel(request.BrandModel);
            return await _brandService.InsertAsync(brand);
        }
    }
}