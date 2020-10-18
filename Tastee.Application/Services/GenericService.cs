using Tastee.Application.Interfaces;
using TrackableEntities.Common.Core;
using URF.Core.Abstractions.Trackable;
using URF.Core.Services;

namespace Tastee.Services
{
    public class GenericService<T> : Service<T>, IGenericService<T> where T : class, ITrackable
    {
        public GenericService(ITrackableRepository<T> repository) : base(repository)
        {
        }
    }
}