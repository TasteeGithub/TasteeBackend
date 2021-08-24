using Mapster;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Domain.Entities;
using Tastee.Shared;

namespace Tastee.Application.Features.Menus.Items.Commands
{
    public class UpdateMenuItemCommand : IRequest<Response>
    {
        public MenuItem MenuItemModel;
    }

    public class UpdateMenuItemCommandHandler : IRequestHandler<UpdateMenuItemCommand, Response>
    {
        private readonly IMenuService _menuService;

        public UpdateMenuItemCommandHandler(IMenuService menuService)
        {
            _menuService = menuService;
        }

        public async Task<Response> Handle(UpdateMenuItemCommand request, CancellationToken cancellationToken)
        {
            var menuItem = _menuService.BuildMenuItemFromMenuItemModel(request.MenuItemModel);
            return await _menuService.UpdateItemAsync(menuItem);
        }
    }
}