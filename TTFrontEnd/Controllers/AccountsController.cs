using LinqKit;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TTBackEnd.Shared;
using TTFrontEnd.Models.DataContext;
//using TTBackEndApi.Models.DataContext;
//using TTFrontEnd.Models.SqlDataContext;
using TTFrontEnd.Services;
using URF.Core.Abstractions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TTFrontEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly ITTService<Users> _serviceUsers;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AccountsController> _logger;

        private readonly IConfiguration _configuration;
        private readonly ITTService<UserRoles> _serviceUserRole;
        private readonly ITTService<Roles> _serviceRoles;

        public AccountsController(
            IConfiguration configuration,
            ILogger<AccountsController> logger,
            IUnitOfWork unitOfWork,
            ITTService<Users> serviceUsers,
            ITTService<UserRoles> serviceUserRole,
            ITTService<Roles> serviceRoles
            )
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _serviceUsers = serviceUsers;
            _serviceRoles = serviceRoles;
            _serviceUserRole = serviceUserRole;
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]RegisterModel model)
        {
            if (UsersIsExists(model.Email))
            {
                return Conflict();
            }

            var passwordHasher = new PasswordHasher<RegisterModel>();
            var passwordHash = passwordHasher.HashPassword(model, model.Password);

            Users newUsers = new Users()
            {
                CreatedDate = DateTime.Now,
                Email = model.Email,
                Id = Guid.NewGuid().ToString(),
                PasswordHash = passwordHash,
                FullName = model.FullName,
            };

            _serviceUsers.Insert(newUsers);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error:{0}", ex.ToString());
                return Ok(new RegisterResult { Successful = false, Errors = new string[] { ex.Message } });
            }
            return Ok(new RegisterResult { Successful = true });
        }

        private bool UsersIsExists(string email)
        {
            return _serviceUsers.Queryable().Any(e => e.Email == email);
        }

        // POST api/<controller>
        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginModel login)
        {
            var passwordHasher = new PasswordHasher<LoginModel>();
            //var passwordHash = passwordHasher.HashPassword(login, login.Password);

            var user = _serviceUsers.Queryable().Where(x => x.Email == login.Email).FirstOrDefault();
            if (user == null) return Ok(new LoginResult { Successful = false, Error = "Username or password are invalid." });
            user.LastLogin = DateTime.Now;
            _serviceUsers.Update(user);
            _unitOfWork.SaveChangesAsync();

            var verifyResult = passwordHasher.VerifyHashedPassword(login, user.PasswordHash, login.Password);
            if (verifyResult == PasswordVerificationResult.Failed) return Ok(new LoginResult { Successful = false, Error = "Username or password are invalid." });

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.FullName));
            claims.Add(new Claim(ClaimTypes.Email, login.Email));

            var roleIdList = _serviceUserRole.Queryable().Where(x => x.UserId == user.Id).ToList();
            foreach (var item in roleIdList)
            {
                claims.Add(new Claim(ClaimTypes.Role, _serviceRoles.Queryable().Where(x => x.Id == item.RoleId).FirstOrDefault().Name));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.Now.AddDays(Convert.ToInt32(_configuration["Jwt:ExpiryInDays"]));

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: expiry,
                signingCredentials: creds
            );

            return Ok(new LoginResult { Successful = true, Token = new JwtSecurityTokenHandler().WriteToken(token) });
        }

        // GET: api/ManageUser
        [HttpGet]
        [Route("GetAccounts")]
        public async Task<PaggingModel<Users>> Get(
            //string userName
            //, string email
            //, string phone, int? status
            //, DateTime? fdate
            //, DateTime? tdate
            //, 

            int pageSize, int? pageIndex
            )
        {
            ExpressionStarter<Users> searCondition = PredicateBuilder.New<Users>(true);

            //if (userName != null && userName.Length > 0)
            //{
            //    searCondition = searCondition.And(x => x.FullName.ToLower().Contains(userName.ToLower()));
            //}
            //if (email != null && email.Length > 0)
            //{
            //    searCondition = searCondition.And(x => x.Email.ToLower().Contains(email.ToLower()));
            //}

            //if (phone != null && phone.Length > 0)
            //{
            //    searCondition = searCondition.And(x => x.Phone.Contains(phone));
            //}
            //if (status != null && status > 0)
            //{
            //    searCondition = searCondition.And(x => x.Status == status);
            //}
            //if (fdate != null && tdate != null)
            //{
            //    searCondition = searCondition.And(x => x.DateCreated >= fdate && x.DateCreated <= tdate);
            //}

            //var listUser = _serviceUsers.QueryableSql($"SELECT * FROM (SELECT [RANK] = ROW_NUMBER() OVER (ORDER BY Id),* FROM Users) A WHERE A.[RANK] BETWEEN {pageIndex} AND {pageSize}");

            var listUser = _serviceUsers.Queryable().Where(searCondition).OrderByDescending(x => x.CreatedDate);

            pageSize = pageSize == 0 ? Constants.DEFAULT_PAGE_SIZE : pageSize;
            var pagedListUser = await PaginatedList<Users>.CreateAsync(listUser, pageIndex ?? 1, pageSize);
            //var pagedListUser = await PaginatedList<Users>.CreateAsync(listUser,pageSize);
            PaggingModel<Users> returnResult = new PaggingModel<Users>()
            {
                ListData = pagedListUser.Adapt<List<Users>>(),
                TotalRows = pagedListUser.TotalRows,
            };

            return returnResult;
        }


        //[HttpGet]
        //[Route("GetAccounts")]
        //public IActionResult GetAccounts(

        //    )
        //{

        //}
    }
}