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
    public class GetGroupItemsQuery : IRequest<PaggingModel<GroupItem>>
    {
        public GetGroupItemViewModel RequestModel { get; set; }
        public class GetGroupItemsQueryHandler : IRequestHandler<GetGroupItemsQuery, PaggingModel<GroupItem>>
        {
            private readonly IGroupItemsService _groupItemsService;

            public GetGroupItemsQueryHandler(IGroupItemsService groupItemsService)
            {
                _groupItemsService = groupItemsService;
            }

            public async Task<PaggingModel<GroupItem>> Handle(GetGroupItemsQuery request, CancellationToken cancellationToken)
            {
                return await _groupItemsService.GetGroupItemsAsync(request.RequestModel);
            }
        }
    }
}