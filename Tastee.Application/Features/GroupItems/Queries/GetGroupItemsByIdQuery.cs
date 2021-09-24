using Mapster;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Application.Wrappers;
using Tastee.Domain.Entities;

namespace Tastee.Features.GroupItems.Queries
{
    public class GetGroupItemsByIdQuery : IRequest<Response<GroupItemDetail>>
    {
        public string Id { get; set; }

        public class GetGroupItemsByIdHandler : IRequestHandler<GetGroupItemsByIdQuery, Response<GroupItemDetail>>
        {
            private readonly IGroupItemsService _groupItemsService;

            public GetGroupItemsByIdHandler(IGroupItemsService groupItemsService)
            {
                _groupItemsService = groupItemsService;
            }

            public async Task<Response<GroupItemDetail>> Handle(GetGroupItemsByIdQuery request, CancellationToken cancellationToken)
            {
                var group = await _groupItemsService.GetByIdAsync(request.Id);
                if (group == null)
                    return new Response<GroupItemDetail>("Group item not found");

                return new Response<GroupItemDetail>(_groupItemsService.BuildGroupItemDetail(group));
            }
        }
    }
}