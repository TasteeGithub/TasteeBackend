using Mapster;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Application.Services;
using Tastee.Application.Wrappers;
using Tastee.Domain.Entities;

namespace Tastee.Features.Toppings.Queries
{
    public class GetToppingByIdQuery : IRequest<Response<Topping>>
    {
        public string Id { get; set; }

        public class GetToppingByIdQueryHandler : IRequestHandler<GetToppingByIdQuery, Response<Topping>>
        {
            private readonly IToppingService _toppingService;

            public GetToppingByIdQueryHandler(IToppingService toppingService)
            {
                _toppingService = toppingService;
            }

            public async Task<Response<Topping>> Handle(GetToppingByIdQuery request, CancellationToken cancellationToken)
            {
                var group = await _toppingService.GetByIdAsync(request.Id);
                if (group == null)
                    return new Response<Topping>("Topping not found");

                return new Response<Topping>(group.Adapt<Topping>());
            }
        }
    }
}