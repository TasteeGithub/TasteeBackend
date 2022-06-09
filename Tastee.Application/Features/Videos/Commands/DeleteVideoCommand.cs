using Mapster;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Shared;

namespace Tastee.Application.Features.Videos.Commands
{
    public class DeleteVideoCommand : IRequest<Response>
    {
        public string Id;
    }

    public class DeleteVideoCommandHandler : IRequestHandler<DeleteVideoCommand, Response>
    {
        private readonly IVideoService _videoService;
       
        public DeleteVideoCommandHandler(IVideoService _ideoService)
        {
            _videoService = _ideoService;
        }

        public async Task<Response> Handle(DeleteVideoCommand request, CancellationToken cancellationToken)
        {
            return await _videoService.DeleteVideoAsync(request.Id);
        }
    }
}