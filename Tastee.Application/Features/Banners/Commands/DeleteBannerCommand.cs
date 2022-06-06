using Mapster;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Shared;

namespace Tastee.Application.Features.Banners.Commands
{
    public class DeleteBannerCommand : IRequest<Response>
    {
        public string Id;
    }

    public class DeleteBannerCommandHandler : IRequestHandler<DeleteBannerCommand, Response>
    {
        private readonly IBannerService _bannersService;
       
        public DeleteBannerCommandHandler(IBannerService bannersService)
        {
            _bannersService = bannersService;
        }

        public async Task<Response> Handle(DeleteBannerCommand request, CancellationToken cancellationToken)
        {
            return await _bannersService.DeleteBannerAsync(request.Id);
        }
    }
}