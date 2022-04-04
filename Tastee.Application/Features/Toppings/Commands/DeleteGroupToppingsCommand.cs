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
    public class DeleteGroupToppingsCommand : IRequest<Response>
    {
        public string Id;
    }

    public class DeleteGroupToppingsCommandHandler : IRequestHandler<DeleteGroupToppingsCommand, Response>
    {
        private readonly IToppingService _toppingService;
       
        public DeleteGroupToppingsCommandHandler(IToppingService toppingService)
        {
            _toppingService = toppingService;
        }

        public async Task<Response> Handle(DeleteGroupToppingsCommand request, CancellationToken cancellationToken)
        {
            return await _toppingService.DeleteGroupToppingsAsync(request.Id);
        }
    }
}