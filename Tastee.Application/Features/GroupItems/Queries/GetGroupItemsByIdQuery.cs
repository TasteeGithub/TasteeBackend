using Mapster;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Application.Wrappers;
using Tastee.Domain.Entities;

namespace Tastee.Features.GroupItems.Queries
{
    public class GetGroupItemsByIdQuery : IRequest<Response<GroupItem>>
    {
        public string Id { get; set; }

        public class GetGroupItemsByIdHandler : IRequestHandler<GetGroupItemsByIdQuery, Response<GroupItem>>
        {
            private readonly IGroupItemsService _groupItemsService;

            public GetGroupItemsByIdHandler(IGroupItemsService groupItemsService)
            {
                _groupItemsService = groupItemsService;
            }

            public async Task<Response<GroupItem>> Handle(GetGroupItemsByIdQuery request, CancellationToken cancellationToken)
            {
                var group = await _groupItemsService.GetByIdAsync(request.Id);
                if (group == null)
                    return new Response<GroupItem>("Group item not found");
                return new Response<GroupItem>(group.Adapt<GroupItem>());
            }
        }
    }
}