using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using TTBackEnd.Shared;
using TTBackEndApi.Models.DataContext;
using TTBackEndApi.Services;
using URF.Core.Abstractions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TTBackEndApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : Controller
    {
        private static UserModel LoggedOutUser = new UserModel { IsAuthenticated = false };

        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITTService<Operator> _serviceOperator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AccountsController> _logger;

        public AccountsController(
            ILogger<AccountsController> logger,
            ITTService<Operator> serviceOperator,
            IUnitOfWork unitOfWork
            )
        {
            _serviceOperator = serviceOperator;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]RegisterModel model)
        {
            if (OperatorExists(model.Email))
            {
                return Conflict();
            }

            Operator op = new Operator()
            {
                CreatedDate = DateTime.Now,
                Email = model.Email,
                Password = model.Password,
                FullName = "Nguyen Minh Thu",
                UserName = model.Email
            };

            _serviceOperator.Insert(op);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error:{0}", ex.ToString());
                return Ok(new RegisterResult { Successful = false, Errors= new string[] { ex.Message } });
            }
            return Ok(new RegisterResult { Successful = true });
        }

        private bool OperatorExists(string email)
        {
            return _serviceOperator.Queryable().Any(e => e.Email == email);
        }
    }
}