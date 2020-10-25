﻿
using System.Collections.Generic;
using System.Threading.Tasks;
using Tastee.Domain.Entities;
using Tastee.Shared;

namespace Tastee.Application.Interfaces
{
    public interface ICityService
    {
        Task<List<City>> GetCitysAsync();
    }
}
