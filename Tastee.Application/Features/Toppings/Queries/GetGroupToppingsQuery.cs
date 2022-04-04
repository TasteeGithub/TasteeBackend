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
    public class GetGroupToppingsQuery : IRequest<PaggingModel<GroupTopping>>
    {
        public GetGroupToppingViewModel RequestModel { get; set; }
        public class GetGroupToppingsQueryHandler : IRequestHandler<GetGroupToppingsQuery, PaggingModel<GroupTopping>>
        {
            private readonly IToppingService _toppingsService;

            public GetGroupToppingsQueryHandler(IToppingService toppingsService)
            {
                _toppingsService = toppingsService;
            }

            public async Task<PaggingModel<GroupTopping>> Handle(GetGroupToppingsQuery request, CancellationToken cancellationToken)
            {
                return await _toppingsService.GetGroupToppingsAsync(request.RequestModel);
            }
        }
    }
}