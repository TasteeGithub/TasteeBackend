using Mapster;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Domain.Entities;
using Tastee.Shared;

namespace Tastee.Application.Features.Menus.Commands
{
    public class CreateMenuCommand : IRequest<Response>
    {
        public Menu MenuModel;
    }

    public class CreateMenuCommandHandler : IRequestHandler<CreateMenuCommand, Response>
    {
        private readonly IMenuService _menuService;

        public CreateMenuCommandHandler(IMenuService menuServic)
        {
            _menuService = menuServic;
        }

        public async Task<Response> Handle(CreateMenuCommand request, CancellationToken cancellationToken)
        {
            var menu = _menuService.BuildMenuFromMenuModel(request.MenuModel);
            return await _menuService.InsertAsync(menu);
        }
    }
}