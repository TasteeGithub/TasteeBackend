using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TTBackEnd.Shared;
using TTBackEndApi.Models.DataContext;
using TTBackEndApi.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TTBackEndApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;
        //private readonly SignInManager<IdentityUser> _signInManager;
        ITTService<Operator> _serviceOperator;
        public LoginController(IConfiguration configuration, ITTService<Operator> serviceOperator)
        {
            _configuration = configuration;
            //_signInManager = signInManager;
            _serviceOperator = serviceOperator;
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            var op = _serviceOperator.Queryable().Where(x => x.Email == login.Email && x.Password == login.Password).FirstOrDefault();

            if (op == null) return BadRequest(new LoginResult { Successful = false, Error = "Username and password are invalid." });

            //var claims = new[]
            //{
            //    new Claim(ClaimTypes.Name, login.Email)
            //};

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, login.Email));

            claims.Add(new Claim(ClaimTypes.Role, "Admin"));


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
    }
}
