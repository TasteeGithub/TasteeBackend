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
    public class DeleteToppingsCommand : IRequest<Response>
    {
        public string Id;
    }

    public class DeleteToppingsCommandHandler : IRequestHandler<DeleteToppingsCommand, Response>
    {
        private readonly IToppingService _toppingService;
       
        public DeleteToppingsCommandHandler(IToppingService toppingService)
        {
            _toppingService = toppingService;
        }

        public async Task<Response> Handle(DeleteToppingsCommand request, CancellationToken cancellationToken)
        {
            return await _toppingService.DeleteToppingsAsync(request.Id);
        }
    }
}