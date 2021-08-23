using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Application.Wrappers;
using Tastee.Domain.Entities;

namespace Tastee.Application.Features.Menus.Queries
{
    public class GetMenuByIdQuery : IRequest<Response<Menu>>
    {
        public string Id { get; set; }

        public class GetMenuByIdQueryHandler : IRequestHandler<GetMenuByIdQuery, Response<Menu>>
        {
            private readonly IMenuService _menuService;

            public GetMenuByIdQueryHandler(IMenuService menuService)
            {
                _menuService = menuService;
            }

            public async Task<Response<Menu>> Handle(GetMenuByIdQuery request, CancellationToken cancellationToken)
            {
                var menu = await _menuService.GetByIdAsync(request.Id);
                return new Response<Menu>(_menuService.BuildMenuModelFromMenu(menu));
            }
        }
    }
}