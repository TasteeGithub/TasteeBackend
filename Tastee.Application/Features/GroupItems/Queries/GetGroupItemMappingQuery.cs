using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Application.ViewModel;
using Tastee.Domain.Entities;
using Tastee.Shared;
using Tastee.Shared.Models.GroupItems;

namespace Tastee.Features.GroupItems.Queries
{
    public class GetGroupItemMappingQuery : IRequest<PaggingModel<GroupItemMappingInfo>>
    {
        public GetGroupItemMappingViewModel RequestModel { get; set; }
        public class GetGroupItemMappingQueryHandler : IRequestHandler<GetGroupItemMappingQuery, PaggingModel<GroupItemMappingInfo>>
        {
            private readonly IGroupItemsService _groupItemsService;

            public GetGroupItemMappingQueryHandler(IGroupItemsService groupItemsService)
            {
                _groupItemsService = groupItemsService;
            }

            public async Task<PaggingModel<GroupItemMappingInfo>> Handle(GetGroupItemMappingQuery request, CancellationToken cancellationToken)
            {
                return await _groupItemsService.GetGroupItemMappingAsync(request.RequestModel);
            }
        }
    }
}