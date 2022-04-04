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
    public class UpdateToppingsCommand : IRequest<Response>
    {
        public UpdateToppingViewModel Model;
        public string UserEmail;
    }

    public class UpdateToppingsCommandHandler : IRequestHandler<UpdateToppingsCommand, Response>
    {
        private readonly IToppingService _toppingService;

        public UpdateToppingsCommandHandler(IToppingService toppingService)
        {
            _toppingService = toppingService;
        }

        public async Task<Response> Handle(UpdateToppingsCommand request, CancellationToken cancellationToken)
        {
            var group = await _toppingService.GetGroupToppingsByIdAsync(request.Model.GroupToppingId);
            if (group == null)
            {
                return new Response { Successful = false, Message = "Group topping not found" };
            }
            var topping = request.Model.Adapt<Tastee.Infrastucture.Data.Context.Toppings>();
            topping.UpdatedDate = Converters.DateTimeToUnixTimeStamp(DateTime.Now).Value;
            topping.UpdatedBy = request.UserEmail;
            return await _toppingService.UpdateAsync(topping);
        }
    }
}