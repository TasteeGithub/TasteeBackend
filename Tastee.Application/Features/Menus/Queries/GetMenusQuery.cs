using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Domain.Entities;
using Tastee.Shared;

namespace Tastee.Application.Features.Menus.Queries
{
    public class GetMenusQuery : IRequest<PaggingModel<Menu>>
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }

        public string MenuName { get; set; }
        public int? Status { get; set; }

        public class GetMenusQueryHandler : IRequestHandler<GetMenusQuery, PaggingModel<Menu>>
        {
            private readonly IMenuService _menuService;
            public GetMenusQueryHandler(IMenuService menuService)
            {
                _menuService = menuService;
            }

            public async Task<PaggingModel<Menu>> Handle(GetMenusQuery request, CancellationToken cancellationToken)
            {
                return await _menuService.GetMenusAsync(request.PageSize, request.PageIndex, request.MenuName, request.Status);
            }

        }
    }
}
