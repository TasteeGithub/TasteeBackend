using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TTFrontEnd.Models.DataContext;
using TTFrontEnd.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TTFrontEnd.Controllers
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
        public IEnumerable<Roles> Get()
        {
            return _serviceRoles.Queryable().ToArray();
        }
    }
}