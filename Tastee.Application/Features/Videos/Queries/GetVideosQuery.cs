using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Domain.Entities;
using Tastee.Shared;
using Tastee.Shared.Models.Videos;

namespace Tastee.Application.Features.Videos.Queries
{
    public class GetVideosQuery : IRequest<PaggingModel<VideoModel>>
    {
        public GetVideosViewModel RequestModel;

        public class GetVideosQueryHandler : IRequestHandler<GetVideosQuery, PaggingModel<VideoModel>>
        {
            private readonly IVideoService _videoService;
            public GetVideosQueryHandler(IVideoService videoService)
            {
                _videoService = videoService;
            }

            public async Task<PaggingModel<VideoModel>> Handle(GetVideosQuery request, CancellationToken cancellationToken)
            {
                return await _videoService.GetVideosAsync(request.RequestModel);
            }

        }
    }
}
