using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TTBackEndApi.Models.DataContext;
using TTBackEndApi.Services;

namespace TTBackEndApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly sw_insideContext _context;
        IRequestService _sv;
        public RequestsController(ILogger<WeatherForecastController> logger, IRequestService sv)
        {
            _sv = sv;
        }

        // GET: api/Requests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IswRequests>>> Get()
        {
            return await _sv.Queryable().Take(5).ToListAsync();// _context.IswRequests.ToListAsync();
        }

        // GET: api/Requests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IswRequests>> Get(string id)
        {
            var iswRequests = await _sv.FindAsync(id);// _context.IswRequests.FindAsync(id);

            if (iswRequests == null)
            {
                return NotFound();
            }

            return iswRequests;
        }

        // PUT: api/Requests/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, IswRequests iswRequests)
        {
            if (id != iswRequests.RequestId)
            {
                return BadRequest();
            }

            _context.Entry(iswRequests).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IswRequestsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Requests
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<IswRequests>> Post(IswRequests iswRequests)
        {
            _context.IswRequests.Add(iswRequests);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (IswRequestsExists(iswRequests.RequestId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetIswRequests", new { id = iswRequests.RequestId }, iswRequests);
        }

        // DELETE: api/Requests/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<IswRequests>> Delete(string id)
        {
            var iswRequests = await _context.IswRequests.FindAsync(id);
            if (iswRequests == null)
            {
                return NotFound();
            }

            _context.IswRequests.Remove(iswRequests);
            await _context.SaveChangesAsync();

            return iswRequests;
        }

        private bool IswRequestsExists(string id)
        {
            return _context.IswRequests.Any(e => e.RequestId == id);
        }
    }
}
