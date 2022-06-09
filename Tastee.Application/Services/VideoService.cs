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
using Tastee.Shared.Models.Videos;
using URF.Core.Abstractions;

namespace Tastee.Application.Services
{
    public class VideoService : IVideoService
    {
        private readonly ILogger<VideoService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        private readonly IGenericService<Videos> _serviceVideo;


        public VideoService(
          ILogger<VideoService> logger,
          IUnitOfWork unitOfWork,
          IGenericService<Videos> serviceVideo)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;

            _serviceVideo = serviceVideo;
        }

        public async Task<Videos> GetByIdAsync(string id)
        {
            return  await _serviceVideo.FindAsync(id);
        }

        public async Task<Response> DeleteVideoAsync(string Id)
        {
      
            var category = await GetByIdAsync(Id);
            if (category == null)
            {
                return new Response { Successful = true, Message = "Delete video successed" };
            }
            _serviceVideo.Delete(category);
            await _unitOfWork.SaveChangesAsync();
            return new Response { Successful = true, Message = "Delete video successed" };
        }

        public async Task<Response> InsertAsync(Videos model)
        {
            model.Id = Guid.NewGuid().ToString();
            _serviceVideo.Insert(model);
            await _unitOfWork.SaveChangesAsync();
            return new Response { Successful = true, Message = "Add video successed" };
        }


        public async Task<Response> UpdateImageAsync(string videoId, string image)
        {
            var catregory = _serviceVideo.Queryable().Where(x => x.Id == videoId).FirstOrDefault();
            catregory.Image = image;
            _serviceVideo.Update(catregory);
            await _unitOfWork.SaveChangesAsync();
            return new Response { Successful = true, Message = "update video image successed" };
        }

        public async Task<Response> UpdateAsync(Videos updateVideo)
        {
            if (updateVideo.Id != null && updateVideo.Id.Length > 0)
            {
                var video = await _serviceVideo.FindAsync(updateVideo.Id);
                if (video == null)
                    return new Response { Successful = false, Message = "Video not found" };


                video.Name = updateVideo.Name ?? video.Name;
                video.Image = updateVideo.Image ?? video.Image;
                video.DisplayOrder = updateVideo.DisplayOrder;
                video.Description = updateVideo.Description ?? video.Description;
                video.Video = updateVideo.Video ?? video.Video;
                video.Type = updateVideo.Type;
                video.IsDisplay = updateVideo.IsDisplay;
                video.UpdatedBy = updateVideo.UpdatedBy;
                video.UpdatedDate = Converters.DateTimeToUnixTimeStamp(DateTime.Now).Value;

                _serviceVideo.Update(video);
                await _unitOfWork.SaveChangesAsync();
                return new Response { Successful = true, Message = "Update video success" };

            }
            return new Response { Successful = false, Message = "Please input id" };
        }

        public async Task<PaggingModel<VideoModel>> GetVideosAsync(GetVideosViewModel requestModel)
        {
            ExpressionStarter<Videos> searchCondition = PredicateBuilder.New<Videos>(true);
            int pageSize = Converters.StringToInteger(requestModel.Length, Constants.DEFAULT_PAGE_SIZE).Value;
            int skip = Converters.StringToInteger(requestModel.Start).Value;
            int pageIndex = skip / pageSize + 1;

            if (!string.IsNullOrEmpty(requestModel.Name))
            {
                searchCondition = searchCondition.And(x => x.Name.ToLower().Contains(requestModel.Name.ToLower()));
            }

            if (requestModel.IsDisplay != null)
            {
                searchCondition = searchCondition.And(x => x.IsDisplay == requestModel.IsDisplay);
            }

            if (requestModel.Type != null)
            {
                searchCondition = searchCondition.And(x => x.Type == requestModel.Type);
            }

            var listVideos = _serviceVideo.Queryable().Where(searchCondition).OrderByDescending(x => x.CreatedDate);

            var pagedListVideos = await PaginatedList<Videos>.CreateAsync(listVideos, pageIndex, pageSize);

            PaggingModel<VideoModel> returnResult = new PaggingModel<VideoModel>()
            {
                ListData = pagedListVideos.Select(x =>  x.Adapt<VideoModel>()).ToList(),
                TotalRows = pagedListVideos.TotalRows,
            };

            return returnResult;
        }
    }
}
