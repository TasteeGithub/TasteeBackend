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

namespace Tastee.Application.Features.Menus.Commands
{
    public class UpdateMenuCommand : IRequest<Response>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string BrandId { get; set; }
        public int Status { get; set; }
        public int Order { get; set; }
        public string UpdatedBy { get; set; }

        public class UpdateMenuCommandHandler : IRequestHandler<UpdateMenuCommand, Response>
        {

            private readonly IMenuService _menuService;
            public UpdateMenuCommandHandler( IMenuService menuService  )
            {
                _menuService = menuService;
            }

            public async Task<Response> Handle(UpdateMenuCommand request, CancellationToken cancellationToken)
            {
                var menuModel = request.Adapt<Menu>();
                return await _menuService.UpdateAsync(menuModel);
            }
        }
    }
}
