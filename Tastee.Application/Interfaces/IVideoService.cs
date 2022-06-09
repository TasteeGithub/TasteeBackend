using System.Threading.Tasks;
using Tastee.Domain.Entities;
using Tastee.Infrastucture.Data.Context;
using Tastee.Shared;
using Tastee.Shared.Models.Videos;

namespace Tastee.Application.Interfaces
{
    public interface IVideoService : ITasteeServices<Videos>
    {
        Task<Response> UpdateImageAsync(string categoryId, string image);
        Task<PaggingModel<VideoModel>> GetVideosAsync(GetVideosViewModel requestModel);
        Task<Response> DeleteVideoAsync(string Id);
    }
}
