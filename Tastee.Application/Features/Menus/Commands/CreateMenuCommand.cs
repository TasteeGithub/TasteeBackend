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
        public string Id { get; set; }
        public string Name { get; set; }
        public string BrandId { get; set; }
        public int Status { get; set; }
        public int Order { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
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
            var menuModel = request.Adapt<Menu>();
            return await _menuService.InsertAsync(menuModel);
        }
    }
}