using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Application.Services;
using Tastee.Domain.Entities;
using Tastee.Shared;
using Tastee.Shared.Models.Topping;

namespace Tastee.Features.Toppings.Queries
{
    public class GetToppingsQuery : IRequest<PaggingModel<Topping>>
    {
        public GetToppingViewModel RequestModel { get; set; }
        public class GetToppingsQueryHandler : IRequestHandler<GetToppingsQuery, PaggingModel<Topping>>
        {
            private readonly IToppingService _toppingsService;

            public GetToppingsQueryHandler(IToppingService toppingsService)
            {
                _toppingsService = toppingsService;
            }

            public async Task<PaggingModel<Topping>> Handle(GetToppingsQuery request, CancellationToken cancellationToken)
            {
                return await _toppingsService.GetToppingsAsync(request.RequestModel);
            }
        }
    }
}