using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tastee.Application.Features.Areas.Queries;
using Tastee.Application.Wrappers;
using Tastee.Domain.Entities;
using Tastee.Feature.Areas.Queries;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Tastee.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AreasController : BaseApiController
    {
        private readonly ILogger<AreasController> _logger;
        public AreasController(
            ILogger<AreasController> logger
            )
        {
            _logger = logger;
        }
        // GET: api/<AreasController>
        [HttpGet]
        public async Task<IActionResult>Get()
        {
            try
            {
                GetAreasQuery areasQuery = new GetAreasQuery();
                var rs = await Mediator.Send(areasQuery);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get Areas");
            }
            return BadRequest();
        }

        // GET api/<AreasController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            try
            {
                GetAreaByIdQuery rq = new GetAreaByIdQuery { Id = id };
                return Ok(await Mediator.Send(rq));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get area detail, area id: {0}", id);
            }
            finally
            {
                _logger.LogInformation("Get area detail, area Id: {0}", id);
            }
            return Ok(new Response<Area>("Has error"));
        }

        [HttpGet()]
        [Route("by-city/{cityId}")]
        public async Task<ActionResult> GetByCity(int cityId)
        {
            try
            {
                GetAreasByCityIdQuery areasByCityIdQuery = new GetAreasByCityIdQuery() { CityId = cityId };
                var rs = await Mediator.Send(areasByCityIdQuery);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get Areas by city Id: {0}", cityId);
            }
            return BadRequest();
        }
    }
}
