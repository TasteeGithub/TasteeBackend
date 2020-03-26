using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using TTBackEnd.Shared;
//using TTBackEndApi.Models.DataContext;
using TTFrontEnd.Models.SqlDataContext;
using TTFrontEnd.Services;
using URF.Core.Abstractions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TTFrontEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsApiController : Controller
    {
        //private static readonly UserModel LoggedOutUser = new UserModel { IsAuthenticated = false };

        //private readonly UserManager<IdentityUser> _userManager;
        private readonly ITTService<Users> _serviceUsers;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AccountsApiController> _logger;

        public AccountsApiController(
            ILogger<AccountsApiController> logger,
            //ITTService<Operator> serviceOperator,
            IUnitOfWork unitOfWork,
            ITTService<Users> serviceUsers
            )
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _serviceUsers = serviceUsers;
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]RegisterModel model)
        {
            if (UsersIsExists(model.Email))
            {
                return Conflict();
            }

            var passwordHasher = new Microsoft.AspNetCore.Identity.PasswordHasher<RegisterModel>();
            var passwordHash = passwordHasher.HashPassword(model, model.Password);

            Users newUsers = new Users()
            {
                CreatedDate = DateTime.Now,
                Email = model.Email,
                Id = Guid.NewGuid().ToString(),
                PasswordHash = passwordHash,
                FullName = model.FullName
            };

            _serviceUsers.Insert(newUsers);

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

        private bool UsersIsExists(string email)
        {
            return _serviceUsers.Queryable().Any(e => e.Email == email);
        }
    }
}