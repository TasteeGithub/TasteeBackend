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
    public class GetGroupToppingByIdQuery : IRequest<Response<GroupTopping>>
    {
        public string Id { get; set; }

        public class GetGroupToppingByIdQueryHandler : IRequestHandler<GetGroupToppingByIdQuery, Response<GroupTopping>>
        {
            private readonly IToppingService _toppingService;

            public GetGroupToppingByIdQueryHandler(IToppingService toppingService)
            {
                _toppingService = toppingService;
            }

            public async Task<Response<GroupTopping>> Handle(GetGroupToppingByIdQuery request, CancellationToken cancellationToken)
            {
                var group = await _toppingService.GetGroupToppingsByIdAsync(request.Id);
                if (group == null)
                    return new Response<GroupTopping>("Group topping not found");

                return new Response<GroupTopping>(group.Adapt<GroupTopping>());
            }
        }
    }
}