using Mapster;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Domain.Entities;
using Tastee.Shared;

namespace Tastee.Features.Banners.Commands
{
    public class UpdateBannerCommand : IRequest<Response>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Image { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public string UpdateBy { get; set; }
        public string BrandId { get; set; }
    }

    public class UpdateBannerCommandHandler : IRequestHandler<UpdateBannerCommand, Response>
    {
        private readonly IBannerService _bannerService;

        public UpdateBannerCommandHandler(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }

        public async Task<Response> Handle(UpdateBannerCommand request, CancellationToken cancellationToken)
        {
            var bannerModel = request.Adapt<Banner>();
            return await _bannerService.UpdateAsync(bannerModel);
        }
    }
}