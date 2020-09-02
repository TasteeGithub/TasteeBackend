using LinqKit;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TTBackEnd.Shared;
using TTFrontEnd.Models.DataContext;
using TTFrontEnd.Models.DTO;

using TTFrontEnd.Services;
using URF.Core.Abstractions;
using Constants = TTBackEnd.Shared.Constants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TTFrontEnd.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OperatorsController: ControllerBase
    {
        private readonly ITTService<Operators> _serviceOperators;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<OperatorsController> _logger;

        private readonly IConfiguration _configuration;
        private readonly ITTService<OperatorRoles> _serviceOperatorRoles;
        private readonly ITTService<Roles> _serviceRoles;

        public OperatorsController(
            IConfiguration configuration,
            ILogger<OperatorsController> logger,
            IUnitOfWork unitOfWork,
            ITTService<Operators> serviceOperators,
            ITTService<OperatorRoles> serviceUserRole,
            ITTService<Roles> serviceRoles
            )
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _serviceOperators = serviceOperators;
            _serviceRoles = serviceRoles;
            _serviceOperatorRoles = serviceUserRole;
        }

        //[HttpPost]
        //public async Task<IActionResult> Post(RegisterModel model)
        //{
        //    return Ok(new RegisterResult { Successful = true });
        //}
        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post(RegisterUserModel model)
        {

            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage);

                return Ok(new RegisterResult { Successful = false, Error = errorMessage });
            }

            if (_serviceOperators.Queryable().Any(e => e.Email == model.Email || e.PhoneNumber == model.PhoneNumber))
            {
                return Ok(new RegisterResult { Successful = false, Error = new string[] { "User is exists" } });
            }

            var passwordHasher = new PasswordHasher<RegisterUserModel>();
            var passwordHash = passwordHasher.HashPassword(model, model.Password);

            Operators newOperators = new Operators()
            {
                CreatedDate = DateTime.Now,
                Email = model.Email,
                Id = Guid.NewGuid().ToString(),
                PasswordHash = passwordHash,
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                Status = model.Status,
            };

            _serviceOperators.Insert(newOperators);

            try
            {
                if ((model.RoleId?.Length ?? 0) > 0)
                {
                    OperatorRoles operatorRoles = new OperatorRoles()
                    {
                        RoleId = model.RoleId,
                        UserId = newOperators.Id
                    };
                    _serviceOperatorRoles.Insert(operatorRoles);
                }

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error:{0}", ex.ToString());
                return Ok(new RegisterResult { Successful = false, Error = new string[] { ex.Message } });
            }
            return Ok(new RegisterResult { Successful = true });
        }

        // POST api/<controller>
        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginModel login)
        {
            bool IsActionSuccess = false;
            try
            {
                var user = _serviceOperators.Queryable().Where(x => x.Email == login.Email).FirstOrDefault();
                
                if (user == null) return Ok(new LoginResult { Successful = false, Error = "Username or password are invalid." });

                if (user.Status == AccountStatus.Locked.ToString() || user.Status == AccountStatus.Closed.ToString())
                {
                    return Ok(new LoginResult { Successful = false, Error = "Username or password are invalid." });
                }    

                var dtoUser = user.Adapt<TTFrontEnd.Models.DTO.User>();
                var verifyResult = dtoUser.VerifyPassword(login);

                if (verifyResult == PasswordVerificationResult.Failed)
                {
                    user.LastLogin = DateTime.Now;
                    user.LoginFailedCount = user.LoginFailedCount == null ? 1 : user.LoginFailedCount + 1;
                    if(user.LoginFailedCount == 5)
                    {
                        user.Status = AccountStatus.Locked.ToString();
                    }    
                    _serviceOperators.Update(user);
                    _unitOfWork.SaveChangesAsync();

                    return Ok(new LoginResult { Successful = false, Error = "Username or password are invalid." });
                }

                user.LastLogin = DateTime.Now;
                user.LoginFailedCount = 0;
                user.Status = AccountStatus.Actived.ToString();
                _serviceOperators.Update(user);
                _unitOfWork.SaveChangesAsync();

                var claims = new List<Claim>();
                claims.Add(new Claim("userId", user.Id));
                claims.Add(new Claim("fullName", user.FullName));
                claims.Add(new Claim("email", login.Email));

                var roleIdList = _serviceOperatorRoles.Queryable().Where(x => x.UserId == user.Id).ToList();
                foreach (var item in roleIdList)
                {
                    claims.Add(new Claim("role", _serviceRoles.Queryable().Where(x => x.Id == item.RoleId).FirstOrDefault().Name));
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
                IsActionSuccess = true;
                return Ok(new LoginResult { Successful = true, Token = new JwtSecurityTokenHandler().WriteToken(token) });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "User Login : {0}", login.Email);
            }
            finally
            {
                _logger.LogInformation("User Login : {0} , Result status: {1}", login.Email, IsActionSuccess ? "Successfull" : "Failed");
            }

            return Ok(new LoginResult { Successful = false, Error = "Login failed" });
        }

        // GET: api/ManageUser
        [HttpGet]
        public async Task<PaggingModel<Operators>> Get(
            int pageSize, int? pageIndex
            , string fullName
            , string email
            , string phone, string status
            , DateTime? fdate
            , DateTime? tdate

            )
        {
            ExpressionStarter<Operators> searCondition = PredicateBuilder.New<Operators>(true);

            if (fullName != null && fullName.Length > 0)
            {
                searCondition = searCondition.And(x => x.FullName.ToLower().Contains(fullName.ToLower()));
            }
            if (email != null && email.Length > 0)
            {
                searCondition = searCondition.And(x => x.Email.ToLower().Contains(email.ToLower()));
            }

            if (phone != null && phone.Length > 0)
            {
                searCondition = searCondition.And(x => x.PhoneNumber.Contains(phone));
            }
            if (status != null && status.Length > 0)
            {
                searCondition = searCondition.And(x => x.Status == status);
            }
            if (fdate != null && tdate != null)
            {
                tdate = new DateTime(tdate.Value.Year, tdate.Value.Month, tdate.Value.Day,23,59,59,990);
                searCondition = searCondition.And(x => x.CreatedDate >= fdate && x.CreatedDate <= tdate);
            }

            var listUser = _serviceOperators.Queryable().Where(searCondition).OrderByDescending(x => x.CreatedDate);

            pageSize = pageSize == 0 ? Constants.DEFAULT_PAGE_SIZE : pageSize;
            var pagedListUser = await PaginatedList<Operators>.CreateAsync(listUser, pageIndex ?? 1, pageSize);
            //var pagedListUser = await PaginatedList<Users>.CreateAsync(listUser,pageSize);
            PaggingModel<Operators> returnResult = new PaggingModel<Operators>()
            {
                ListData = pagedListUser.Adapt<List<Operators>>(),
                TotalRows = pagedListUser.TotalRows,
            };

            return returnResult;
        }

        [HttpGet]
        [Route("Detail/{id}")]
        public async Task<UserDetail> GetOperatorDetail(
            string id
            )
        {
            try
            {
                var user = await _serviceOperators.FindAsync(id);
                var role = _serviceOperatorRoles.Queryable().Where(x => x.UserId == user.Id).FirstOrDefault();

                var userDetail = user.Adapt<UserDetail>();
                if (role != null) userDetail.RoleId = role.RoleId;
                return userDetail;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get Operator detail, Operatorid: {0}", id);
            }
            finally
            {
                _logger.LogInformation("Get Operator detail, OperatorId {0}", id);
            }
            return null;
        }

        [HttpPut]
        public async Task<IActionResult> Put(UserDetail model)
        {
            bool isActionSuccess = false;
            try
            {
                if (model.Id != null && model.Id.Length > 0)
                {
                    var user = await _serviceOperators.FindAsync(model.Id);
                    if (user != null)
                    {
                        user.FullName = model.FullName;
                        user.PhoneNumber = model.PhoneNumber;
                        user.Status = model.Status;
                        if(user.Status == AccountStatus.Actived.ToString())
                        {
                            user.LoginFailedCount = 0;
                        }    
                        _serviceOperators.Update(user);

                        if ((model.RoleId?.Length ?? 0) > 0)
                        {
                            var roleUser = _serviceOperatorRoles.Queryable().Where(x => x.UserId == model.Id).FirstOrDefault();

                            if (roleUser != null)
                            {
                                _serviceOperatorRoles.Delete(roleUser);
                            }

                            roleUser = new OperatorRoles()
                            {
                                UserId = model.Id,
                                RoleId = model.RoleId
                            };
                            _serviceOperatorRoles.Insert(roleUser);
                        }
                        await _unitOfWork.SaveChangesAsync();
                        isActionSuccess = true;
                        return Ok(new { Successful = true });
                    }
                    else
                    {
                        isActionSuccess = true;
                        return Ok(new { Successful = false, Error = "User not found" });
                    }
                }
                isActionSuccess = true;
                return Ok(new { Successful = false, Error = "Please input id" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update user, Operator: {0}", model);
            }
            finally
            {
                _logger.LogInformation("Update user, Operator: {0}, Result status: ", model, isActionSuccess);
            }
            return Ok(new { Successful = false, Error = "Has error when update" });
        }

        [HttpPost]
        [Route("load-data")]
        public async Task<IActionResult> LoadData(
            [FromForm] string draw,
            [FromForm] string start,
            [FromForm] string length,
            [FromForm] string name,
            [FromForm] string email,
            [FromForm] string phone,
            [FromForm] DateTime? fromDate,
            [FromForm] DateTime? toDate,
            [FromForm] string status
            )
        {
            try
            {
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int pageIndex = skip / pageSize + 1;
                int recordsTotal = 0;

                var rs = await Get(pageSize, pageIndex, name, email, phone, status, fromDate, toDate);

                //total number of rows counts
                recordsTotal = rs.TotalRows;

                //Paging
                var data = rs.ListData; //customerData.Skip(skip).Take(pageSize).ToList();

                //Returning Json Data
                return new JsonResult(
                    new { draw, recordsFiltered = recordsTotal, recordsTotal, data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LoadData");
            }

            return new JsonResult(
                    new { draw, recordsFiltered = 0, recordsTotal = 0, data = new List<Users>() });
        }

        [HttpPut]
        [Route("set-password")]
        public async Task<IActionResult> SetPassword(SetPasswordRequest passwordRequest)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage);

                return Ok(new RegisterResult { Successful = false, Error = errorMessage });
            }
            var user = await _serviceOperators.FindAsync(passwordRequest.Id);
            if (user != null)
            {
                var userDto = user.Adapt<User>();
                string hasPassword = userDto.SetPassword(passwordRequest.Password);
                user.PasswordHash = hasPassword;
                _serviceOperators.Update(user);
                await _unitOfWork.SaveChangesAsync();

                return Ok(new { Successful = true, Error = "Set password successfull" });
            }
            return Ok(new { Successful = false, Error = "Set password failed" });
        }

        [HttpPut]
        [Route("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest passwordRequest)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage);

                return Ok(new RegisterResult { Successful = false, Error = errorMessage });
            }

            var user = await _serviceOperators.FindAsync(passwordRequest.Id);
            if (user != null)
            {
                var userDto = user.Adapt<User>();
                var verifyResult = userDto.VerifyPassword(new LoginModel()
                {
                    Email = user.Email,
                    Password = passwordRequest.Password
                });

                if (verifyResult == PasswordVerificationResult.Success)
                {
                    string hasPassword = userDto.SetPassword(passwordRequest.NewPassword);
                    user.PasswordHash = hasPassword;
                    _serviceOperators.Update(user);
                    await _unitOfWork.SaveChangesAsync();

                    return Ok(new { Successful = true, Error = "Change password successfull" });
                }

                return Ok(new { Successful = false, Error = "Password is incorrect !" });
            }
            return Ok(new { Successful = false, Error = "" });
        }
    }
}