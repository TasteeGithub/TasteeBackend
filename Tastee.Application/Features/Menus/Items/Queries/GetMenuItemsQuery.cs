using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Domain.Entities;
using Tastee.Shared;

namespace Tastee.Application.Features.Menus.Items.Queries
{
    public class GetMenuItemsQuery : IRequest<PaggingModel<MenuItem>>
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }

        public string ItemName { get; set; }
        public int? Status { get; set; }

        public class GetMenuItemsQueryHandler : IRequestHandler<GetMenuItemsQuery, PaggingModel<MenuItem>>
        {
            private readonly IMenuService _menuService;
            public GetMenuItemsQueryHandler(IMenuService menuService)
            {
                _menuService = menuService;
            }

            public async Task<PaggingModel<MenuItem>> Handle(GetMenuItemsQuery request, CancellationToken cancellationToken)
            {
                return await _menuService.GetMenuItemsAsync(request.PageSize, request.PageIndex, request.ItemName, request.Status);
            }

        }
    }
}
