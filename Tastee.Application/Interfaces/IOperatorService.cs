using System;
using System.Collections.Generic;
using System.Text;
using Tastee.Application.ViewModel;

namespace Tastee.Application.Interfaces
{
    public interface IOperatorService
    {
        OperatorViewModel GetOperators();
        
    }
}
