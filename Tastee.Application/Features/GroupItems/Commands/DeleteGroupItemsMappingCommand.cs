using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
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
    public class DeleteGroupItemsMappingCommand : IRequest<Response>
    {
        public DeleteGroupItemMappingViewModel Model;
    }

    public class DeleteGroupItemsMappingCommandHandler : IRequestHandler<DeleteGroupItemsMappingCommand, Response>
    {
        private readonly IGroupItemsService _groupItemService;
       
        public DeleteGroupItemsMappingCommandHandler(IGroupItemsService groupItemService)
        {
            _groupItemService = groupItemService;
        }

        public async Task<Response> Handle(DeleteGroupItemsMappingCommand request, CancellationToken cancellationToken)
        {
            var group = await _groupItemService.GetByIdAsync(request.Model.GroupId);
            if (group == null)
                return new Response() { Successful = false, Message = "Group item not found" };
            return await _groupItemService.DeleteGroupItemMapping(request.Model.MenuItemIds, request.Model.GroupId);
        }
    }
}