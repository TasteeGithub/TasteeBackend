using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Application.Wrappers;
using Tastee.Domain.Entities;

namespace Tastee.Application.Features.MenuItems.Queries
{
    public class GetMenuItemByIdQuery : IRequest<Response<MenuItem>>
    {
        public string Id { get; set; }

        public class GetMenuItemByIdQueryHandler : IRequestHandler<GetMenuItemByIdQuery, Response<MenuItem>>
        {
            private readonly IMenuItemService _menuItemService;
            public GetMenuItemByIdQueryHandler(IMenuItemService menuItemService )
            {
                _menuItemService = menuItemService;
            }
            public async Task<Response<MenuItem>> Handle(GetMenuItemByIdQuery request, CancellationToken cancellationToken)
            {
                var menu = await _menuItemService.GetByIdAsync(request.Id);
                return new Response<MenuItem>(menu);
            }
        }
    }
}
