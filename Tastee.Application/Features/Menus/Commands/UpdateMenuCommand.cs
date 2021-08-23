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
    public class UpdateMenuCommand : IRequest<Response>
    {
        public Menu MenuModel;
    }

    public class UpdateMenuCommandHandler : IRequestHandler<UpdateMenuCommand, Response>
    {
        private readonly IMenuService _menuService;

        public UpdateMenuCommandHandler(IMenuService menuService)
        {
            _menuService = menuService;
        }

        public async Task<Response> Handle(UpdateMenuCommand request, CancellationToken cancellationToken)
        {
            var menu = _menuService.BuildMenuFromMenuModel(request.MenuModel);
            return await _menuService.UpdateAsync(menu);
        }
    }
}