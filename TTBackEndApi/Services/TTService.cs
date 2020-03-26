using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTFrontEnd.Models.DataContext;
using URF.Core.Services;
using URF.Core.Abstractions.Trackable;
using TrackableEntities.Common.Core;

namespace TTFrontEnd.Services
{
    public class TTService<T> : Service<T> , ITTService<T> where T : class, ITrackable
    {
        public TTService(ITrackableRepository<T> repository): base(repository)
        {
        }
    }
}
