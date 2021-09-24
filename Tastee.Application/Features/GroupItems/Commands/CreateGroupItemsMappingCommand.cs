using Mapster;
using MediatR;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Application.ViewModel;
using Tastee.Domain.Entities;
using Tastee.Shared;
using Tastee.Shared.Models.GroupItems;

namespace Tastee.Features.GroupItems.Commands
{
    public class CreateGroupItemsMappingCommand : IRequest<Response>
    {
        public InsertGroupItemMappingViewModel Model;
        public string UserEmail;
    }

    public class CreateGroupItemsMappingCommandHandler : IRequestHandler<CreateGroupItemsMappingCommand, Response>
    {
        private readonly IGroupItemsService _groupItemService;
        private readonly IBrandService _brandService;

        public CreateGroupItemsMappingCommandHandler(IGroupItemsService groupItemService, IBrandService brandService)
        {
            _groupItemService = groupItemService;
            _brandService = brandService;
        }

        public async Task<Response> Handle(CreateGroupItemsMappingCommand request, CancellationToken cancellationToken)
        {
            var insertModel = request.Model;
            var group = await _groupItemService.GetByIdAsync(insertModel.GroupItemsId);
            if (group == null)
                return new Response() { Successful = false, Message = "Group item not found" };
            var rs = await _brandService.CheckMenuItemsBelongBrand(insertModel.ListItemId, group.BrandId);
            if (!rs.Successful)
                return rs;
           return await _groupItemService.InsertGroupItemMappingAsync(insertModel.ListItemId, insertModel.GroupItemsId, request.UserEmail);
        }
    }
}