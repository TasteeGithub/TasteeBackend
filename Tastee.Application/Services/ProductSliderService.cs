using LinqKit;
using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Domain.Entities;
using Tastee.Infrastucture.Data.Context;
using Tastee.Shared;
using URF.Core.Abstractions;

namespace Tastee.Application.Services
{
    public class ProductSliderService : IProductSliderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProductSliderService> _logger;

        private readonly IGenericService<ProductSliders> _serviceProductSliders;

        public ProductSliderService(
           ILogger<ProductSliderService> logger,
           IUnitOfWork unitOfWork,
           IGenericService<ProductSliders> serviceProductSliders
           )
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _serviceProductSliders = serviceProductSliders;
        }

        public async Task<ProductSlider> GetByIdAsync(string id)
        {
            var ProductSlider = await _serviceProductSliders.FindAsync(id);
            return ProductSlider.Adapt<ProductSlider>();
        }

        public async Task<PaggingModel<ProductSlider>> GetProductSlidersAsync(int pageSize, int? pageIndex, string brandId, DateTime? fromDate, DateTime? toDate, string status)
        {
            ExpressionStarter<ProductSliders> searchCondition = PredicateBuilder.New<ProductSliders>(true);

            if ((brandId ?? string.Empty).Length > 0)
            {
                searchCondition = searchCondition.And(x => x.BrandId == brandId);
            }

            if (fromDate != null)
            {
                searchCondition = searchCondition.And(x => x.CreatedDate >= fromDate);
            }

            if (toDate != null)
            {
                searchCondition = searchCondition.And(x => x.CreatedDate <= toDate);
            }

            if ((status ?? string.Empty).Length > 0)
            {
                searchCondition = searchCondition.And(x => x.Status  == status );
            }    

            var listProductSliders = _serviceProductSliders.Queryable().Where(searchCondition).OrderByDescending(x => x.CreatedDate);

            var pagedListUser = await PaginatedList<ProductSliders>.CreateAsync(listProductSliders, pageIndex ?? 1, pageSize);

            PaggingModel<ProductSlider> returnResult = new PaggingModel<ProductSlider>()
            {
                ListData = pagedListUser.Adapt<List<ProductSlider>>(),
                TotalRows = pagedListUser.TotalRows,
            };

            return returnResult;
        }

        public async Task<Response> InsertAsync(ProductSlider model)
        {
            //if (!_serviceProductSliders.Queryable().Any(x => x.Name == model.Name))
            //{
            ProductSliders newProductSliders = model.Adapt<ProductSliders>();
            newProductSliders.Id = Guid.NewGuid().ToString();
            newProductSliders.Status = ProductSliderStatus.Pending.ToString();
            newProductSliders.CreatedDate = DateTime.Now;
            _serviceProductSliders.Insert(newProductSliders);
            await _unitOfWork.SaveChangesAsync();
            return new Response { Successful = true, Message = "Add ProductSlider successed" };
            //}
            //return new Response { Successful = false, Message = "ProductSlider is exists" };
        }

        public async Task<Response> UpdateAsync(ProductSlider model)
        {
            if (model.Id != null && model.Id.Length > 0)
            {
                var ProductSlider = await _serviceProductSliders.FindAsync(model.Id);
                if (ProductSlider != null)
                {
                    ProductSlider.BrandId = model.BrandId ?? ProductSlider.BrandId;
                    ProductSlider.Order = model.Order;
                    ProductSlider.StartDate = model.StartDate;
                    ProductSlider.EndDate = model.EndDate;
                    ProductSlider.UpdateDate = DateTime.Now;
                    ProductSlider.Status = model.Status ?? ProductSlider.Status;
                    ProductSlider.Note = model.Note ?? ProductSlider.Note;
                    ProductSlider.Image = model.Image ?? ProductSlider.Image;
                    ProductSlider.UpdateBy = model.UpdateBy ?? ProductSlider.UpdateBy;

                    _serviceProductSliders.Update(ProductSlider);
                    await _unitOfWork.SaveChangesAsync();

                    return new Response { Successful = true, Message = "Update ProductSlider success" };
                }
                else
                {
                    return new Response { Successful = false, Message = "ProductSlider not found" };
                }
            }
            return new Response { Successful = false, Message = "Please input id" };
        }
    }
}