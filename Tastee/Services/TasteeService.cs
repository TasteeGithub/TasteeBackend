using TrackableEntities.Common.Core;
using URF.Core.Abstractions.Trackable;
using URF.Core.Services;

namespace Tastee.Services
{
    public class TasteeService<T> : Service<T>, ITasteeService<T> where T : class, ITrackable
    {
        public TasteeService(ITrackableRepository<T> repository) : base(repository)
        {
        }
    }
}