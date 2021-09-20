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

namespace Tastee.Features.GroupItems.Commands
{
    public class CreateGroupItemsCommand : IRequest<Response>
    {
        public GroupItem GroupItemModel;
    }

    public class CreateGroupItemsCommandHandler : IRequestHandler<CreateGroupItemsCommand, Response>
    {
        private readonly IGroupItemsService _groupItemService;
        private readonly IBrandService _brandService;

        public CreateGroupItemsCommandHandler(IGroupItemsService groupItemService, IBrandService brandService)
        {
            _groupItemService = groupItemService;
            _brandService = brandService;
        }

        public async Task<Response> Handle(CreateGroupItemsCommand request, CancellationToken cancellationToken)
        {
            var brand = await _brandService.GetByIdAsync(request.GroupItemModel.BrandId);
            if (brand == null)
                return new Response() { Successful = false, Message = "Brand not found" };
            var group = request.GroupItemModel.Adapt<Tastee.Infrastucture.Data.Context.GroupItems>();
            return await _groupItemService.InsertAsync(group);
        }
    }
}