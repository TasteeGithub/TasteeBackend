using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TTFrontEnd.Models.DataContext;
using TTFrontEnd.Services;
using URF.Core.Abstractions;

namespace TTFrontEnd
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperatorController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        readonly ITTService<Operator> _serviceOperator;
        public OperatorController(ILogger<OperatorController> logger, ITTService<Operator> serviceOperator, IUnitOfWork unitOfWork)
        {
            _serviceOperator = serviceOperator;
            _unitOfWork = unitOfWork;
        }

        // GET: api/Operator
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Operator>>> Get()
        {
            return await _serviceOperator.Queryable().ToListAsync();
        }

        // GET: api/Operator/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<ActionResult<Operator>> Get(long id)
        {
            var oper = await _serviceOperator.FindAsync(id);

            if (oper == null)
            {
                return NotFound();
            }

            return oper;
        }

        // POST: api/Operator
        [HttpPost]
        public async Task<ActionResult<Operator>> Post(Operator oper)
        {
            _serviceOperator.Insert(oper);
            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (OperatorExists(oper.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            return CreatedAtAction("Get", new { id = oper.UserId }, oper);
        }

        // PUT: api/Requests/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, Operator oper)
        {
            if (id != oper.UserId)
            {
                return BadRequest();
            }
            _serviceOperator.Update(oper);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OperatorExists(id))
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

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _serviceOperator.DeleteAsync(id);

            if (!result)
                return NotFound();

            await _unitOfWork.SaveChangesAsync();

            return StatusCode((int)HttpStatusCode.NoContent);
        }

        private bool OperatorExists(long id)
        {
            return _serviceOperator.Queryable().Any(e => e.UserId == id);
        }
    }
}
