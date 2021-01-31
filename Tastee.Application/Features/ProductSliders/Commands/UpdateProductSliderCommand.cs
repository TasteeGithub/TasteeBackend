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
    public class UpdateProductSliderCommand : IRequest<Response>
    {
        public string Id { get; set; }
        public string BrandId { get; set; }
        public int Order { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public string Image { get; set; }
        public string UpdateBy { get; set; }
    }

    public class UpdateProductSliderCommandHandler : IRequestHandler<UpdateProductSliderCommand, Response>
    {
        private readonly IProductSliderService _ProductSliderService;

        public UpdateProductSliderCommandHandler(IProductSliderService ProductSliderService)
        {
            _ProductSliderService = ProductSliderService;
        }

        public async Task<Response> Handle(UpdateProductSliderCommand request, CancellationToken cancellationToken)
        {
            var ProductSliderModel = request.Adapt<ProductSlider>();
            return await _ProductSliderService.UpdateAsync(ProductSliderModel);
        }
    }
}