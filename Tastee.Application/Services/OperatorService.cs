using Mapster;
using System.Collections.Generic;
using System.Linq;
using Tastee.Application.Interfaces;
using Tastee.Application.ViewModel;
using Tastee.Infrastucture.Data.Context;

namespace Tastee.Application.Services
{
    public class OperatorService : IOperatorService
    {
        private readonly IGenericService<Operators> _serviceOperators;

        public OperatorService(IGenericService<Operators> serviceOperators)
        {
            _serviceOperators = serviceOperators;
        }

        public OperatorViewModel GetOperators()
        {
            return new OperatorViewModel()
            {
                Operators = _serviceOperators.Queryable().ToList().Adapt<List<Domain.Entities.Operator>>()
            };
        }
    }
}