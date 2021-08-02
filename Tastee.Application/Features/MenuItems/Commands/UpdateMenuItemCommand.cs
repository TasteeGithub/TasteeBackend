using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Application.Wrappers;
using Tastee.Domain.Entities;
using Tastee.Shared;

namespace Tastee.Application.Features.MenuItems.Commands
{
    public class UpdateMenuItemCommand : IRequest<Response>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string MenuId { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public decimal Price { get; set; }
        public int LikeNumber { get; set; }
        public int SaleNumber { get; set; }
        public int Status { get; set; }
        public int Order { get; set; }
        public string UpdatedBy { get; set; }

        public class UpdateMenuItemCommandHandler : IRequestHandler<UpdateMenuItemCommand, Response>
        {

            private readonly IMenuItemService _menuItemService;
            public UpdateMenuItemCommandHandler( IMenuItemService menuItemService  )
            {
                _menuItemService = menuItemService;
            }

            public async Task<Response> Handle(UpdateMenuItemCommand request, CancellationToken cancellationToken)
            {
                var menuItemModel = request.Adapt<MenuItem>();
                return await _menuItemService.UpdateAsync(menuItemModel);
            }
        }
    }
}
