using Mapster;
using MediatR;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Application.Services;
using Tastee.Application.ViewModel;
using Tastee.Domain.Entities;
using Tastee.Infrastucture.Data.Context;
using Tastee.Shared;

namespace Tastee.Features.Toppings.Commands
{
    public class CreateToppingsCommand : IRequest<Response>
    {
        public Topping ToppingsModel;
    }

    public class CreateToppingsCommandHandler : IRequestHandler<CreateToppingsCommand, Response>
    {
        private readonly IToppingService _toppingService;

        public CreateToppingsCommandHandler(IToppingService toppingService)
        {
            _toppingService = toppingService;
        }

        public async Task<Response> Handle(CreateToppingsCommand request, CancellationToken cancellationToken)
        {
            var group = await _toppingService.GetGroupToppingsByIdAsync(request.ToppingsModel.GroupToppingId);
            if (group == null)
                return new Response() { Successful = false, Message = "Group topping not found" };
            return await _toppingService.InsertAsync(request.ToppingsModel.Adapt<Tastee.Infrastucture.Data.Context.Toppings>());
        }
    }
}