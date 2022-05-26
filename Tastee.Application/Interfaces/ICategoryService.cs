using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tastee.Domain.Entities;
using Tastee.Infrastucture.Data.Context;
using Tastee.Shared;
using Tastee.Shared.Models.Categories;

namespace Tastee.Application.Interfaces
{
    public interface ICategoryService : ITasteeServices<Categories>
    {
        Task<Response> UpdateImageAsync(string categoryId, string image);
        Task<PaggingModel<Category>> GetCategoriesAsync(GetCategoriesViewModel requestModel);
    }
}
