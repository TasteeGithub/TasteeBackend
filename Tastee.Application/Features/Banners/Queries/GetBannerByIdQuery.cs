using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Application.Wrappers;
using Tastee.Domain.Entities;

namespace Tastee.Feature.Banners.Queries
{
    public class GetBannerByIdQuery : IRequest<Response<Banner>>
    {
        public string Id { get; set; }

        public class GetBannerByIdQueryHandler : IRequestHandler<GetBannerByIdQuery, Response<Banner>>
        {
            private readonly IBannerService _bannerService;

            public GetBannerByIdQueryHandler(IBannerService bannerService)
            {
                _bannerService = bannerService;
            }

            public async Task<Response<Banner>> Handle(GetBannerByIdQuery request, CancellationToken cancellationToken)
            {
                var banner = await _bannerService.GetByIdAsync(request.Id);
                return new Response<Banner>(banner);
            }
        }
    }
}