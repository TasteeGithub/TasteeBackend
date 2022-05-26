using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Domain.Entities;
using Tastee.Shared;
using Tastee.Shared.Models.Categories;

namespace Tastee.Application.Features.Categories.Queries
{
    public class GetCategoriesQuery : IRequest<PaggingModel<Category>>
    {
        public GetCategoriesViewModel RequestModel;

        public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, PaggingModel<Category>>
        {
            private readonly ICategoryService _categoryService;
            public GetCategoriesQueryHandler(ICategoryService categoryService)
            {
                _categoryService = categoryService;
            }

            public async Task<PaggingModel<Category>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
            {
                return await _categoryService.GetCategoriesAsync(request.RequestModel);
            }

        }
    }
}
