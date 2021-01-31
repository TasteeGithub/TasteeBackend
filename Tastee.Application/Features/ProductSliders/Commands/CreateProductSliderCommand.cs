using Mapster;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Domain.Entities;
using Tastee.Shared;

namespace Tastee.Features.ProductSliders.Commands
{
    public class CreateProductSliderCommand : IRequest<Response>
    {

        public string BrandId { get; set; }
        public int Order { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public string Image { get; set; }
        public string CreatedBy { get; set; }
    }

    public class CreateProductSliderCommandHandler : IRequestHandler<CreateProductSliderCommand, Response>
    {
        private readonly IProductSliderService _ProductSliderService;

        public CreateProductSliderCommandHandler(IProductSliderService ProductSliderService)
        {
            _ProductSliderService = ProductSliderService;
        }

        public async Task<Response> Handle(CreateProductSliderCommand request, CancellationToken cancellationToken)
        {
            var ProductSliderModel = request.Adapt<ProductSlider>();
            return await _ProductSliderService.InsertAsync(ProductSliderModel);
        }
    }
}