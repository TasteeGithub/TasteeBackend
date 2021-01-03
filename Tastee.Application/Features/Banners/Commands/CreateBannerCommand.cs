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
    public class CreateBannerCommand : IRequest<Response>
    {
        public string Name { get; set; }
        public string Link { get; set; }
        public string Image { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public string CreatedBy { get; set; }
        public string BrandId { get; set; }
    }

    public class CreateBannerCommandHandler : IRequestHandler<CreateBannerCommand, Response>
    {
        private readonly IBannerService _bannerService;

        public CreateBannerCommandHandler(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }

        public async Task<Response> Handle(CreateBannerCommand request, CancellationToken cancellationToken)
        {
            var bannerModel = request.Adapt<Banner>();
            return await _bannerService.InsertAsync(bannerModel);
        }
    }
}