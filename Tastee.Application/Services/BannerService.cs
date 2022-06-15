using LinqKit;
using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Application.Utilities;
using Tastee.Domain.Entities;
using Tastee.Infrastucture.Data.Context;
using Tastee.Shared;
using URF.Core.Abstractions;

namespace Tastee.Application.Services
{
    public class BannerService : IBannerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<BannerService> _logger;

        private readonly IGenericService<Banners> _serviceBanners;

        public BannerService(
           ILogger<BannerService> logger,
           IUnitOfWork unitOfWork,
           IGenericService<Banners> serviceBanners
           )
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _serviceBanners = serviceBanners;
        }

        public async Task<Banner> GetByIdAsync(string id)
        {
            var banner = await _serviceBanners.FindAsync(id);
            return banner.Adapt<Banner>();
        }

        public async Task<PaggingModel<BannerSimple>> GetBannersAsync(int pageSize, int? pageIndex, string name, long? fromDate, long? toDate, string status, int? type, bool? isDisplay)
        {
            ExpressionStarter<Banners> searchCondition = PredicateBuilder.New<Banners>(true);

            if ((name ?? string.Empty).Length > 0)
            {
                searchCondition = searchCondition.And(x => x.Name.ToLower().Contains(name.ToLower()));
            }

            if (fromDate != null)
            {
                searchCondition = searchCondition.And(x => x.StartDate >= fromDate);
            }

            if (toDate != null)
            {
                searchCondition = searchCondition.And(x => x.EndDate <= toDate);
            }

            if ((status ?? string.Empty).Length > 0)
            {
                searchCondition = searchCondition.And(x => x.Status == status);
            }

            if (type != null)
            {
                searchCondition = searchCondition.And(x => x.Type == type.Value);
            }

            if (isDisplay != null)
            {
                searchCondition = searchCondition.And(x => x.IsDisplay == isDisplay.Value);
            }

            var listBanners = _serviceBanners.Queryable().Where(searchCondition).OrderByDescending(x => x.CreatedDate);

            var pagedListUser = await PaginatedList<Banners>.CreateAsync(listBanners, pageIndex ?? 1, pageSize);

            PaggingModel<BannerSimple> returnResult = new PaggingModel<BannerSimple>()
            {
                ListData = pagedListUser.Adapt<List<BannerSimple>>(),
                TotalRows = pagedListUser.TotalRows,
            };

            return returnResult;
        }

        public async Task<Response> InsertAsync(Banner model)
        {
            if (!_serviceBanners.Queryable().Any(x => x.Name == model.Name))
            {
                Banners newBanners = model.Adapt<Banners>();
                _serviceBanners.Insert(newBanners);
                await _unitOfWork.SaveChangesAsync();
                return new Response { Successful = true, Message = "Add banner successed" };
            }
            return new Response { Successful = false, Message = "Banner is exists" };
        }

        public async Task<Response> UpdateAsync(Banner model)
        {
            if (model.Id != null && model.Id.Length > 0)
            {
                var banner = await _serviceBanners.FindAsync(model.Id);
                if (banner != null)
                {
                    banner.Name = model.Name ?? banner.Name;
                    banner.Link = model.Link ?? banner.Link;
                    banner.Image = model.Image ?? banner.Image;
                    banner.StartDate = model.StartDate;
                    banner.EndDate = model.EndDate;
                    banner.UpdateDate = Converters.DateTimeToUnixTimeStamp(DateTime.Now).Value;
                    banner.Status = model.Status ?? banner.Status;
                    banner.Note = model.Note ?? banner.Note;
                    banner.UpdateBy = model.UpdateBy ?? banner.UpdateBy;
                    banner.BrandId = model.BrandId ?? banner.BrandId;
                    banner.DisplayOrder = model.DisplayOrder;
                    banner.Type = model.Type;
                    banner.Navigation = model.Navigation;
                    banner.IsDisplay = model.IsDisplay;
                    banner.Description = model.Description ?? banner.Description;

                    _serviceBanners.Update(banner);
                    await _unitOfWork.SaveChangesAsync();

                    return new Response { Successful = true, Message = "Update Banner success" };
                }
                else
                {
                    return new Response { Successful = false, Message = "Banner not found" };
                }
            }
            return new Response { Successful = false, Message = "Please input id" };
        }

        public async Task<Response> DeleteBannerAsync(string Id)
        {

            var banner = await _serviceBanners.FindAsync(Id);
            if (banner == null)
            {
                return new Response { Successful = true, Message = "Delete banner successed" };
            }
            _serviceBanners.Delete(banner);
            await _unitOfWork.SaveChangesAsync();
            return new Response { Successful = true, Message = "Delete banner successed" };
        }

        public async Task<Response> UpdateImageAsync(string id, string url)
        {
            var banner = _serviceBanners.Queryable().Where(x => x.Id == id).FirstOrDefault();
            banner.Image = url;
            _serviceBanners.Update(banner);
            await _unitOfWork.SaveChangesAsync();
            return new Response { Successful = true, Message = "update banner image successed" };
        }
    }
}