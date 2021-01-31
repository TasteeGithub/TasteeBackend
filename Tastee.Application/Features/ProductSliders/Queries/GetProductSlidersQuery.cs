using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Domain.Entities;
using Tastee.Shared;

namespace Tastee.Features.ProductSliders.Queries
{
    public class GetProductSlidersQuery : IRequest<PaggingModel<ProductSlider>>
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string BrandId { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Status { get; set; }

        public class GetProductSlidersQueryHandler : IRequestHandler<GetProductSlidersQuery, PaggingModel<ProductSlider>>
        {
            private readonly IProductSliderService _ProductSliderService;

            public GetProductSlidersQueryHandler(IProductSliderService ProductSliderService)
            {
                _ProductSliderService = ProductSliderService;
            }

            public async Task<PaggingModel<ProductSlider>> Handle(GetProductSlidersQuery request, CancellationToken cancellationToken)
            {
                return await _ProductSliderService.GetProductSlidersAsync(request.PageSize, request.PageIndex, request.BrandId, request.FromDate,request.ToDate,request.Status);
            }
        }
    }
}