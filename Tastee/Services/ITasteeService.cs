using TrackableEntities.Common.Core;
using URF.Core.Abstractions.Services;

namespace Tastee.Services
{
    public interface ITasteeService<T> : IService<T> where T : class, ITrackable
    {
    }
}