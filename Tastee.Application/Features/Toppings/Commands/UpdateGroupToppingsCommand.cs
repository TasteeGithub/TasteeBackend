using Mapster;
using MediatR;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Application.Services;
using Tastee.Application.Utilities;
using Tastee.Application.ViewModel;
using Tastee.Domain.Entities;
using Tastee.Shared;
using Tastee.Shared.Models.GroupItems;

namespace Tastee.Features.Toppings.Commands
{
    public class UpdateGroupToppingsCommand : IRequest<Response>
    {
        public UpdateGroupToppingViewModel Model;
        public string UserEmail;
    }

    public class UpdateGroupToppingsCommandHandler : IRequestHandler<UpdateGroupToppingsCommand, Response>
    {
        private readonly IToppingService _toppingService;

        public UpdateGroupToppingsCommandHandler(IToppingService toppingService)
        {
            _toppingService = toppingService;
        }

        public async Task<Response> Handle(UpdateGroupToppingsCommand request, CancellationToken cancellationToken)
        {
            var groupTopping = request.Model.Adapt<Tastee.Infrastucture.Data.Context.GroupToppings>();
            groupTopping.UpdatedDate = Converters.DateTimeToUnixTimeStamp(DateTime.Now).Value;
            groupTopping.UpdatedBy = request.UserEmail;
            return await _toppingService.UpdateGroupToppingsAsync(groupTopping);
        }
    }
}