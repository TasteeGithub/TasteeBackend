using Mapster;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Shared;

namespace Tastee.Application.Features.Categories.Commands
{
    public class DeleteCategoryCommand : IRequest<Response>
    {
        public string Id;
    }

    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Response>
    {
        private readonly ICategoryService _categoriesService;
       
        public DeleteCategoryCommandHandler(ICategoryService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        public async Task<Response> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            return await _categoriesService.DeleteCategoryAsync(request.Id);
        }
    }
}