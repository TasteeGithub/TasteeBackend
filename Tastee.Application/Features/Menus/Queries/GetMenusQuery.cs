using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Application.ViewModel;
using Tastee.Domain.Entities;
using Tastee.Shared;

namespace Tastee.Application.Features.Menus.Queries
{
    public class GetMenusQuery : IRequest<PaggingModel<Menu>>
    {
        public GetMenusViewModel RequestModel;

        public class GetMenusQueryHandler : IRequestHandler<GetMenusQuery, PaggingModel<Menu>>
        {
            private readonly IMenuService _menuService;
            public GetMenusQueryHandler(IMenuService menuService)
            {
                _menuService = menuService;
            }

            public async Task<PaggingModel<Menu>> Handle(GetMenusQuery request, CancellationToken cancellationToken)
            {
                return await _menuService.GetMenusAsync(request.RequestModel);
            }

        }
    }
}
