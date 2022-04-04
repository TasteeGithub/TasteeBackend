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
    public class CreateGroupToppingsCommand : IRequest<Response>
    {
        public GroupTopping GroupToppingsModel;
    }

    public class CreateGroupToppingsCommandHandler : IRequestHandler<CreateGroupToppingsCommand, Response>
    {
        private readonly IToppingService _toppingService;

        public CreateGroupToppingsCommandHandler(IToppingService toppingService)
        {
            _toppingService = toppingService;
        }

        public async Task<Response> Handle(CreateGroupToppingsCommand request, CancellationToken cancellationToken)
        {
            return await _toppingService.InsertGroupToppingsAsync(request.GroupToppingsModel.Adapt<GroupToppings>());
        }
    }
}