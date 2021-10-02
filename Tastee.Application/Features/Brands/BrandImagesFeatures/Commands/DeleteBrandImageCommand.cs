using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Application.Utilities;
using Tastee.Shared;

namespace Tastee.Application.Features.Brands.BrandImagesFeatures.Commands
{
    public class DeleteBrandImageCommand : IRequest<Response>
    {
        public string ID;
    }

    public class DeleteBrandImageCommandHandler : IRequestHandler<DeleteBrandImageCommand, Response>
    {
        private readonly IBrandService _brandService;
        private readonly IFileService _fileService;

        public DeleteBrandImageCommandHandler(IBrandService brandService, IFileService fileService)
        {
            _brandService = brandService;
            _fileService = fileService;
        }

        public async Task<Response> Handle(DeleteBrandImageCommand request, CancellationToken cancellationToken)
        {
            var image = await _brandService.GetBrandImageByIdAsync(request.ID);
            if (image == null)
                return new Response { Successful = false, Message = "BrandImage not found" };
            var msg = await _fileService.DeleteFromS3BucketAsync(image.Image);
            if (!String.IsNullOrEmpty(msg))
                return new Response { Successful = false, Message = "Delete S3 Image failed" };
            return await _brandService.DeleteBrandImagesAsync(request.ID);
        }
    }
}