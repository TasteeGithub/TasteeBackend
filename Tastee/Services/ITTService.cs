using TrackableEntities.Common.Core;
using URF.Core.Abstractions.Services;

namespace Tastee.Services
{
    public interface ITTService<T> : IService<T> where T : class, ITrackable
    {
    }
}