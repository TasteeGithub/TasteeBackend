using TrackableEntities.Common.Core;
using URF.Core.Abstractions.Services;

namespace Tastee.Application.Interfaces
{
    public interface IGenericService<T> : IService<T> where T : class, ITrackable
    {
    }
}