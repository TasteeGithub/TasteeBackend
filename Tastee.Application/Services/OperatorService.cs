using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tastee.Application.Interfaces;
using Tastee.Application.ViewModel;
using Tastee.Infrastucture.Data.Context;
using Tastee.Services;

namespace Tastee.Application.Services
{
    public class OperatorService : IOperatorService
    {
        private readonly ITasteeService<Operators> _serviceOperators;
        public OperatorService(ITasteeService<Operators> serviceOperators)
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
