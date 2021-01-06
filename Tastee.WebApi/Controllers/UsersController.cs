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
using Tastee.Application.Interfaces;
using Tastee.Infrastucture.Data.Context;
using Tastee.Models.DTO;
using Tastee.Shared;
using URF.Core.Abstractions;
using Constants = Tastee.Shared.Constants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Tastee.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IGenericService<Users> _serviceUsers;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UsersController> _logger;

        private readonly IConfiguration _configuration;

        public UsersController(
            IConfiguration configuration,
            ILogger<UsersController> logger,
            IUnitOfWork unitOfWork,
            IGenericService<Users> serviceUsers
            )
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _serviceUsers = serviceUsers;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post(RegisterUserModel model)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage);

                return Ok(new Response { Successful = false, Message = string.Join(',', errorMessage) });
            }

            if (_serviceUsers.Queryable().Any(e => e.Email == model.Email || e.PhoneNumber == model.PhoneNumber))
            {
                return Ok(new Response { Successful = false, Message = "User is exists" });
            }

            var passwordHasher = new PasswordHasher<RegisterUserModel>();
            var passwordHash = passwordHasher.HashPassword(model, model.Password);

            Users newUsers = new Users()
            {
                CreatedDate = DateTime.Now,
                Email = model.Email,
                Id = Guid.NewGuid().ToString(),
                PasswordHash = passwordHash,
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                Status = model.Status,
                Address = model.Address,
                Avatar = model.Avatar,
                Birthday = model.Birthday,
                Gender = model.Gender
            };

            _serviceUsers.Insert(newUsers);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error:{0}", ex.ToString());
                return Ok(new Response { Successful = false, Message = ex.Message });
            }
            return Ok(new Response { Successful = true });
        }

        [HttpPost, DisableRequestSizeLimit]
        [Route("uploadfile")]
        public IActionResult UploadFile()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("ClientApp", "public", "Images", "Avatar");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim().ToString();

                    FileInfo fileInfo = new FileInfo(fileName);
                    string newFileName = System.IO.Path.GetRandomFileName() + fileInfo.Extension;

                    var fullPath = Path.Combine(pathToSave, newFileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    return Ok(new { newFileName });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
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
                var user = _serviceUsers.Queryable().Where(x => x.Email == login.Email).FirstOrDefault();

                if (user == null) return Ok(new LoginResult { Successful = false, Error = "Username or password are invalid." });

                if (user.Status == AccountStatus.Locked.ToString() || user.Status == AccountStatus.Closed.ToString())
                {
                    return Ok(new LoginResult { Successful = false, Error = "Username or password are invalid." });
                }

                var dtoUser = user.Adapt<Tastee.Models.DTO.User>();
                var verifyResult = dtoUser.VerifyPassword(login);

                if (verifyResult == PasswordVerificationResult.Failed)
                {
                    user.LastLogin = DateTime.Now;
                    user.LoginFailedCount = user.LoginFailedCount == null ? 1 : user.LoginFailedCount + 1;
                    if (user.LoginFailedCount == 5)
                    {
                        user.Status = AccountStatus.Locked.ToString();
                    }
                    _serviceUsers.Update(user);
                    _unitOfWork.SaveChangesAsync();

                    return Ok(new LoginResult { Successful = false, Error = "Username or password are invalid." });
                }

                user.LastLogin = DateTime.Now;
                user.LoginFailedCount = 0;
                user.Status = AccountStatus.Active.ToString();
                _serviceUsers.Update(user);
                _unitOfWork.SaveChangesAsync();

                var claims = new List<Claim>
                {
                    new Claim("userId", user.Id),
                    new Claim("fullName", user.FullName),
                    new Claim("email", login.Email)
                };

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
        public async Task<PaggingModel<Users>> Get(
            int pageSize, int? pageIndex
            , string fullName
            , string email
            , string phone, string status
            , DateTime? fdate
            , DateTime? tdate

            )
        {
            ExpressionStarter<Users> searCondition = PredicateBuilder.New<Users>(true);

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
                tdate = new DateTime(tdate.Value.Year, tdate.Value.Month, tdate.Value.Day, 23, 59, 59, 990);
                searCondition = searCondition.And(x => x.CreatedDate >= fdate && x.CreatedDate <= tdate);
            }

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

        [HttpGet]
        [Route("Detail/{id}")]
        public async Task<UserDetail> GetUserDetail(
            string id
            )
        {
            try
            {
                var user = await _serviceUsers.FindAsync(id);

                var userDetail = user.Adapt<UserDetail>();

                return userDetail;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get User detail, Userid: {0}", id);
            }
            finally
            {
                _logger.LogInformation("Get User detail, UserId {0}", id);
            }
            return null;
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Put(UserDetail model)
        {
            bool isActionSuccess = false;
            try
            {
                if (model.Id != null && model.Id.Length > 0)
                {
                    var user = await _serviceUsers.FindAsync(model.Id);
                    if (user != null)
                    {
                        user.FullName = model.FullName;
                        user.PhoneNumber = model.PhoneNumber;
                        user.Status = model.Status;
                        user.Gender = model.Gender;
                        user.Address = model.Address;
                        user.Avatar = model.Avatar;
                        user.Birthday = model.Birthday;

                        if (user.Status == AccountStatus.Active.ToString())
                        {
                            user.LoginFailedCount = 0;
                        }
                        _serviceUsers.Update(user);

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
                _logger.LogError(ex, "Update user, User: {0}", model);
            }
            finally
            {
                _logger.LogInformation("Update user, User: {0}, Result status: {1}", model, isActionSuccess);
            }
            return Ok(new { Successful = false, Error = "Has error when update" });
        }

        /// <summary>
        /// Dùng phần trang trong table.net
        /// </summary>
        /// <param name="draw"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="phone"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="status"></param>
        /// <returns></returns>
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
                var data = rs.ListData;

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

        [HttpPost]
        [Route("set-password")]
        public async Task<IActionResult> SetPassword(SetPasswordRequest passwordRequest)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage);

                return Ok(new Response { Successful = false, Message = string.Join(',', errorMessage) });
            }
            var user = await _serviceUsers.FindAsync(passwordRequest.Id);
            if (user != null)
            {
                var userDto = user.Adapt<User>();
                string hasPassword = userDto.SetPassword(passwordRequest.Password);
                user.PasswordHash = hasPassword;
                _serviceUsers.Update(user);
                await _unitOfWork.SaveChangesAsync();

                return Ok(new { Successful = true, Error = "Set password successfull" });
            }
            return Ok(new { Successful = false, Error = "Set password failed" });
        }

        [HttpPost]
        [Route("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest passwordRequest)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage);

                return Ok(new Response { Successful = false, Message = string.Join(',', errorMessage) });
            }

            var user = await _serviceUsers.FindAsync(passwordRequest.Id);
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
                    _serviceUsers.Update(user);
                    await _unitOfWork.SaveChangesAsync();

                    return Ok(new { Successful = true, Error = "Change password successfull" });
                }

                return Ok(new { Successful = false, Error = "Password is incorrect !" });
            }
            return Ok(new { Successful = false, Error = "" });
        }
    }
}