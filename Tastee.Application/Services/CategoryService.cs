using LinqKit;
using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Application.Utilities;
using Tastee.Domain.Entities;
using Tastee.Infrastucture.Data.Context;
using Tastee.Shared;
using Tastee.Shared.Models.Categories;
using URF.Core.Abstractions;

namespace Tastee.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ILogger<CategoryService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        private readonly IGenericService<Categories> _serviceCategory;


        public CategoryService(
          ILogger<CategoryService> logger,
          IUnitOfWork unitOfWork,
          IGenericService<Categories> serviceCategory)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;

            _serviceCategory = serviceCategory;
        }

        public async Task<Categories> GetByIdAsync(string id)
        {
            return  await _serviceCategory.FindAsync(id);
        }

        public async Task<Response> DeleteCategoryAsync(string Id)
        {
      
            var category = await GetByIdAsync(Id);
            if (category == null)
            {
                return new Response { Successful = true, Message = "Delete category successed" };
            }
            _serviceCategory.Delete(category);
            await _unitOfWork.SaveChangesAsync();
            return new Response { Successful = true, Message = "Delete category successed" };
        }

        public async Task<Response> InsertAsync(Categories model)
        {
            if(_serviceCategory.Queryable().Any(x => x.Id == model.Id))
                return new Response { Successful = false, Message = "category existed" };
            _serviceCategory.Insert(model);
            await _unitOfWork.SaveChangesAsync();
            return new Response { Successful = true, Message = "Add category successed" };
        }


        public async Task<Response> UpdateImageAsync(string categoryId, string image)
        {
            var catregory = _serviceCategory.Queryable().Where(x => x.Id == categoryId).FirstOrDefault();
            catregory.Image = image;
            _serviceCategory.Update(catregory);
            await _unitOfWork.SaveChangesAsync();
            return new Response { Successful = true, Message = "update category image successed" };
        }

        public Task<Response> UpdateAsync(Categories model)
        {
            throw new NotImplementedException();
        }

        public async Task<PaggingModel<Category>> GetCategoriesAsync(GetCategoriesViewModel requestModel)
        {
            ExpressionStarter<Categories> searchCondition = PredicateBuilder.New<Categories>(true);
            int pageSize = Converters.StringToInteger(requestModel.Length, Constants.DEFAULT_PAGE_SIZE).Value;
            int skip = Converters.StringToInteger(requestModel.Start).Value;
            int pageIndex = skip / pageSize + 1;

            if (!string.IsNullOrEmpty(requestModel.Name))
            {
                searchCondition = searchCondition.And(x => x.Name.ToLower().Contains(requestModel.Name.ToLower()));
            }

            if (!string.IsNullOrEmpty(requestModel.Id))
            {
                searchCondition = searchCondition.And(x => x.Id == requestModel.Id);
            }

            if (requestModel.Type != null)
            {
                searchCondition = searchCondition.And(x => x.Type == requestModel.Type);
            }

            var listCategories = _serviceCategory.Queryable().Where(searchCondition).OrderByDescending(x => x.CreatedDate);

            var pagedListMenus = await PaginatedList<Categories>.CreateAsync(listCategories, pageIndex, pageSize);

            PaggingModel<Category> returnResult = new PaggingModel<Category>()
            {
                ListData = pagedListMenus.Select(x =>  x.Adapt<Category>()).ToList(),
                TotalRows = pagedListMenus.TotalRows,
            };

            return returnResult;
        }
    }
}
