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
    public class CreateMenuItemCommand : IRequest<Response>
    {

        public MenuItem MenuItemModel;
    }

    public class CreateMenuItemCommandHandler : IRequestHandler<CreateMenuItemCommand, Response>
    {
        private readonly IMenuService _menuService;

        public CreateMenuItemCommandHandler(IMenuService menuServic)
        {
            _menuService = menuServic;
        }

        public async Task<Response> Handle(CreateMenuItemCommand request, CancellationToken cancellationToken)
        {
            var menuItem = _menuService.BuildMenuItemFromMenuItemModel(request.MenuItemModel);
            return await _menuService.InsertItemAsync(menuItem);
        }
    }
}