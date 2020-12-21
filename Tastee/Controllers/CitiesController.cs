using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Tastee.Application.Wrappers;
using Tastee.Domain.Entities;
using Tastee.Feature.Brands.Queries;
using Tastee.Features.Brands.Commands;
using Tastee.Features.Brands.Queries;
using Tastee.Features.Cities.Queries;
using Tastee.Infrastucture.Data.Context;
using Tastee.Shared;

namespace Tastee.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : BaseApiController
    {
        private readonly ILogger<CitiesController> _logger;

        public CitiesController(
            ILogger<CitiesController> logger
            )
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            )
        {
            try
            {
                GetCitiesQuery citiesQuery = new GetCitiesQuery();
                var rs = await Mediator.Send(citiesQuery);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get Cities");
            }
            return BadRequest();
        }
    }
}