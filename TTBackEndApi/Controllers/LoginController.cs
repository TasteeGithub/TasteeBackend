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
using TTFrontEnd.Models.SqlDataContext;
using TTFrontEnd.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TTFrontEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;

        //private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ITTService<Operator> _serviceOperator;

        private readonly ITTService<Users> _serviceUsers;
        private readonly ITTService<UserRoles> _serviceUserRole;
        private readonly ITTService<Roles> _serviceRoles;
        private readonly ILogger<LoginController> _logger;

        public LoginController(IConfiguration configuration,
            ITTService<Operator> serviceOperator,
            ITTService<Users> serviceUsers,
            ITTService<UserRoles> serviceUserRole,
            ITTService<Roles> serviceRoles,
            ILogger<LoginController> logger
            )
        {
            _configuration = configuration;
            //_signInManager = signInManager;
            _serviceOperator = serviceOperator;
            _serviceUsers = serviceUsers;
            _serviceRoles = serviceRoles;
            _serviceUserRole = serviceUserRole;
            _logger = logger;
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var passwordHasher = new Microsoft.AspNetCore.Identity.PasswordHasher<LoginModel>();
                    var passwordHash = passwordHasher.HashPassword(login, login.Password);

                    var user = _serviceUsers.Queryable().Where(x => x.Email == login.Email).FirstOrDefault();

                    if (user == null) return BadRequest(new LoginResult { Successful = false, Error = "Username and password are invalid." });

                    var verifyResult = passwordHasher.VerifyHashedPassword(login, user.PasswordHash, login.Password);
                    if (verifyResult == PasswordVerificationResult.Failed) return BadRequest(new LoginResult { Successful = false, Error = "Username and password are invalid." });

                    var claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.Name, user.FullName));
                    claims.Add(new Claim(ClaimTypes.Email, login.Email));

                    var roleIdList = _serviceUserRole.Queryable().Where(x => x.UserId == user.Id).ToList();
                    foreach (var item in roleIdList)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, _serviceRoles.Queryable().Where(x => x.Id == item.RoleId).FirstOrDefault().Name));
                    }

                    //claims.Add(new Claim(ClaimTypes.Role, "Admin"));

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var expiry = DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["Jwt:ExpiryInDays"]));

                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: expiry,
                        signingCredentials: creds
                    );
                    _logger.LogInformation("Logging: Email:{0}", login.Email);
                    return Ok(new LoginResult { Successful = true, Token = new JwtSecurityTokenHandler().WriteToken(token) });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error:{0}", ex.ToString());
                return BadRequest(new LoginResult { Successful = false, Error = "Có lỗi" });
            }

            return BadRequest(new LoginResult { Successful = false, Error = "Có lỗi" });
        }
    }
}