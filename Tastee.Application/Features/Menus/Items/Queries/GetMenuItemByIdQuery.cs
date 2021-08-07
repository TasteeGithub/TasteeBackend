using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Application.Wrappers;
using Tastee.Domain.Entities;

namespace Tastee.Application.Features.Menus.Items.Queries
{
    public class GetMenuItemByIdQuery : IRequest<Response<MenuItem>>
    {
        public string Id { get; set; }

        public class GetMenuItemByIdQueryHandler : IRequestHandler<GetMenuItemByIdQuery, Response<MenuItem>>
        {
            private readonly IMenuService _menuService;

            public GetMenuItemByIdQueryHandler(IMenuService menuService)
            {
                _menuService = menuService;
            }

            public async Task<Response<MenuItem>> Handle(GetMenuItemByIdQuery request, CancellationToken cancellationToken)
            {
                var menuItem = await _menuService.GetItemByIdAsync(request.Id);
                return new Response<MenuItem>(menuItem);
            }
        }
    }
}