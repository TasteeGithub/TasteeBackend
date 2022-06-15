using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Domain.Entities;
using Tastee.Shared;

namespace Tastee.Features.Banners.Queries
{
    public class GetBannersQuery : IRequest<PaggingModel<BannerSimple>>
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string BannerName { get; set; }

        public long? FromDate { get; set; }
        public long? ToDate { get; set; }
        public string Status { get; set; }
        public int? Type { get; set; }
        public bool? IsDisplay { get; set; }

        public class GetBannersQueryHandler : IRequestHandler<GetBannersQuery, PaggingModel<BannerSimple>>
        {
            private readonly IBannerService _bannerService;

            public GetBannersQueryHandler(IBannerService bannerService)
            {
                _bannerService = bannerService;
            }

            public async Task<PaggingModel<BannerSimple>> Handle(GetBannersQuery request, CancellationToken cancellationToken)
            {
                return await _bannerService.GetBannersAsync(request.PageSize, request.PageIndex, request.BannerName, request.FromDate, request.ToDate, request.Status, request.Type, request.IsDisplay);
            }
        }
    }
}