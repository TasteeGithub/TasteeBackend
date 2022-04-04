using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Shared;
using Tastee.Shared.Models.Brands;


namespace Tastee.Application.Features.Brands.Commands
{
    public class UpdateSuggestBrandCommand : IRequest<Response>
    {
        public UpdateSuggestBrandModel Model;
        public string Email;
    }

    public class UpdateSuggestBrandCommandHandler : IRequestHandler<UpdateSuggestBrandCommand, Response>
    {
        private readonly IBrandService _brandService;

        public UpdateSuggestBrandCommandHandler(IBrandService brandService)
        {
            _brandService = brandService;
        }

        public async Task<Response> Handle(UpdateSuggestBrandCommand request, CancellationToken cancellationToken)
        {
            return await _brandService.UpdateSuggestBrandAsync(request.Model, request.Email);
        }
    }
}