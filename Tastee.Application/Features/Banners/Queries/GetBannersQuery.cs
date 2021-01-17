using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Domain.Entities;
using Tastee.Shared;

namespace Tastee.Features.Banners.Queries
{
    public class GetBannersQuery : IRequest<PaggingModel<Banner>>
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string BannerName { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Status { get; set; }

        public class GetBannersQueryHandler : IRequestHandler<GetBannersQuery, PaggingModel<Banner>>
        {
            private readonly IBannerService _bannerService;

            public GetBannersQueryHandler(IBannerService bannerService)
            {
                _bannerService = bannerService;
            }

            public async Task<PaggingModel<Banner>> Handle(GetBannersQuery request, CancellationToken cancellationToken)
            {
                return await _bannerService.GetBannersAsync(request.PageSize, request.PageIndex, request.BannerName,request.FromDate,request.ToDate,request.Status);
            }
        }
    }
}