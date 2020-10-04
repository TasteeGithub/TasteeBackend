using Mapster;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Domain.Entities;
using Tastee.Shared;

namespace Tastee.Features.Brands.Commands
{
    public class CreateBrandCommand : IRequest<Response>
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Hotline { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string HeadOffice { get; set; }
        public string Uri { get; set; }
        public string Logo { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
        public string Status { get; set; }
        public string MetaDescription { get; set; }
        public string SeoTitle { get; set; }
        public string SeoDescription { get; set; }
        public string SeoImage { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string Cuisines { get; set; }
        public string Categories { get; set; }
    }

    public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, Response>
    {
        private readonly IBrandService _brandService;

        public CreateBrandCommandHandler(IBrandService brandService)
        {
            _brandService = brandService;
        }

        public async Task<Response> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
            var brandModel = request.Adapt<Brand>();
            return await _brandService.InsertAsync(brandModel);
        }
    }
}