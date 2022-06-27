using Mapster;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Application.Services;
using Tastee.Shared;

namespace Tastee.Application.Features.Notficaitions.Commands
{
    public class DeleteNotficaitionsCommand : IRequest<Response>
    {
        public string Id;
    }

    public class DeleteNotficaitionsCommandHandler : IRequestHandler<DeleteNotficaitionsCommand, Response>
    {
        private readonly INotificationService _notificationsService;
       
        public DeleteNotficaitionsCommandHandler(INotificationService notificationsService)
        {
            _notificationsService = notificationsService;
        }

        public async Task<Response> Handle(DeleteNotficaitionsCommand request, CancellationToken cancellationToken)
        {
            return await _notificationsService.DeleteCategoryAsync(request.Id);
        }
    }
}