using Mapster;
using MediatR;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Application.Utilities;
using Tastee.Application.ViewModel;
using Tastee.Domain.Entities;
using Tastee.Shared;
using Tastee.Shared.Models.GroupItems;

namespace Tastee.Features.GroupItems.Commands
{
    public class UpdateGroupItemsCommand : IRequest<Response>
    {
        public UpdateGroupItemViewModel Model;
        public string UserEmail;
    }

    public class UpdateGroupItemsCommandHandler : IRequestHandler<UpdateGroupItemsCommand, Response>
    {
        private readonly IGroupItemsService _groupItemsService;

        public UpdateGroupItemsCommandHandler(IGroupItemsService groupItemsService)
        {
            _groupItemsService = groupItemsService;
        }

        public async Task<Response> Handle(UpdateGroupItemsCommand request, CancellationToken cancellationToken)
        {
            var groupItem = request.Model.Adapt<Tastee.Infrastucture.Data.Context.GroupItems>();
            groupItem.UpdatedDate = Converters.DateTimeToUnixTimeStamp(DateTime.Now).Value;
            groupItem.UpdatedBy = request.UserEmail;
            return await _groupItemsService.UpdateAsync(groupItem);
        }
    }
}