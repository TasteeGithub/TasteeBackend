using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Tastee.Features.Cities.Queries;

namespace Tastee.WebApi.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> Get()
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