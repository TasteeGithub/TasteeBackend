using TrackableEntities.Common.Core;
using URF.Core.Abstractions.Trackable;
using URF.Core.Services;

namespace TTFrontEnd.Services
{
    public class TTService<T> : Service<T>, ITTService<T> where T : class, ITrackable
    {
        public TTService(ITrackableRepository<T> repository) : base(repository)
        {
        }
    }
}