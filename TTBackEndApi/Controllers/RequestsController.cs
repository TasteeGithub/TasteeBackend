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
using URF.Core.Abstractions.Services;

namespace TTBackEndApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly SW_InsideContext _context;
        IService<Operator> _sv;
        public RequestsController(ILogger<RequestsController> logger, IService<Operator> sv)
        {
            _sv = sv;
        }

        // GET: api/Requests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Operator>>> Get()
        {
            return await _sv.Queryable().ToListAsync();
            //return await _sv.Queryable().Take(5).ToListAsync();// _context.IswRequests.ToListAsync();
        }

        // GET: api/Requests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Operator>> Get(string id)
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
        public async Task<IActionResult> Put(int id, Operator iswRequests)
        {
            if (id != iswRequests.UserId)
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
        public async Task<ActionResult<Operator>> Post(Operator iswRequests)
        {
            _context.Operator.Add(iswRequests);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (IswRequestsExists(iswRequests.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetIswRequests", new { id = iswRequests.UserId }, iswRequests);
        }

        // DELETE: api/Requests/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Operator>> Delete(string id)
        {
            var iswRequests = await _context.Operator.FindAsync(id);
            if (iswRequests == null)
            {
                return NotFound();
            }

            _context.Operator.Remove(iswRequests);
            await _context.SaveChangesAsync();

            return iswRequests;
        }

        private bool IswRequestsExists(long id)
        {
            return _context.Operator.Any(e => e.UserId == id);
        }
    }
}
