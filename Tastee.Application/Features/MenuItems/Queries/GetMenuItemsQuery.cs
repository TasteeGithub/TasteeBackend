using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Domain.Entities;
using Tastee.Shared;

namespace Tastee.Application.Features.MenuItems.Queries
{
    public class GetMenuItemsQuery : IRequest<PaggingModel<MenuItem>>
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public class GetMenuItemsQueryHandler : IRequestHandler<GetMenuItemsQuery, PaggingModel<MenuItem>>
        {
            private readonly IMenuItemService _menuItemService;
            public GetMenuItemsQueryHandler(IMenuItemService menuItemService)
            {
                _menuItemService = menuItemService;
            }
            public async Task<PaggingModel<MenuItem>> Handle(GetMenuItemsQuery request, CancellationToken cancellationToken)
            {
                return await _menuItemService.GetMenuItemsAsync(request.PageSize, request.PageIndex,request.Name,request.Status);
            }
        }
    }
}
