using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Application.Utilities;
using Tastee.Shared;

namespace Tastee.Application.Features.Brands.BrandDecorationFeatures.Commands
{
    public class DeleteDecorationImageCommand : IRequest<Response>
    {
        public string ID;
    }

    public class DeleteDecorationImageCommandHandler : IRequestHandler<DeleteDecorationImageCommand, Response>
    {
        private readonly IBrandService _brandService;
        private readonly IFileService _fileService;

        public DeleteDecorationImageCommandHandler(IBrandService brandService, IFileService fileService)
        {
            _brandService = brandService;
            _fileService = fileService;
        }

        public async Task<Response> Handle(DeleteDecorationImageCommand request, CancellationToken cancellationToken)
        {
            var image = await _brandService.GetWidgetImageByIdAsync(request.ID);
            if (image == null)
                return new Response { Successful = false, Message = "Deocration Image not found" };
            var msg = await _fileService.DeleteFromS3BucketAsync(image.Image);
            if (!String.IsNullOrEmpty(msg))
                return new Response { Successful = false, Message = "Delete S3 Image failed" };
            return await _brandService.DeleteDecorationImageAsync(request.ID);
        }
    }
}