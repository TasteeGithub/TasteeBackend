using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Tastee.Models.SqlDataContext;
using Tastee.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Tastee.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly ITTService<Roles> _serviceRoles;

        public RolesController(
            ITTService<Roles> serviceRoles
            )
        {
            _serviceRoles = serviceRoles;
        }

        // GET: api/<RolesController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_serviceRoles.Queryable().ToArray());
        }
    }
}