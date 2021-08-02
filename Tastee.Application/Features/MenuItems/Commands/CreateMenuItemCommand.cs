using Mapster;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Domain.Entities;
using Tastee.Shared;

namespace Tastee.Application.Features.MenuItems.Commands
{
    public class CreateMenuItemCommand : IRequest<Response>
    {
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
        public string CreatedBy { get; set; }

        public class CreateMenuItemCommandHandler : IRequestHandler<CreateMenuItemCommand,Response>
        {
            private readonly IMenuItemService _menuItemService;

            public CreateMenuItemCommandHandler(IMenuItemService menuItemService)
            {
                _menuItemService = menuItemService;
            }

            public async Task<Response> Handle(CreateMenuItemCommand request, CancellationToken cancellationToken)
            {
                var menuItemModel = request.Adapt<MenuItem>();
                return await _menuItemService.InsertAsync(menuItemModel);
            }

        }
    }
}