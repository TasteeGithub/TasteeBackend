using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTBackEndApi.Models.DataContext;
using URF.Core.Services;
using URF.Core.Abstractions.Trackable;

namespace TTBackEndApi.Services
{
    public class RequestService : Service<IswRequests> , IRequestService
    {
        public RequestService(ITrackableRepository<IswRequests> repository): base(repository)
        {
        }
    }
}
