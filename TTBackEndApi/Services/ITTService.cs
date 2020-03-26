using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackableEntities.Common.Core;
using TTFrontEnd.Models.DataContext;
using URF.Core.Abstractions.Services;

namespace TTFrontEnd.Services
{
    public interface ITTService<T> : IService<T> where T : class, ITrackable
    {
    }
}
