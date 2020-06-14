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

//using TTBackEndApi.Models.DataContext;
//using TTFrontEnd.Models.SqlDataContext;
using TTFrontEnd.Services;
using URF.Core.Abstractions;
using Constants = TTBackEnd.Shared.Constants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TTFrontEnd.Controllers
{
    [Authorize]
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

            if (_serviceUsers.Queryable().Any(e => e.Email == model.Email || e.PhoneNumber == model.PhoneNumber))
            {
                return Ok(new RegisterResult { Successful = false, Error = new string[] { "User is exists" } });
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
                Address = model.Address,
                Avatar = model.Avatar,
                Birthday = model.Birthday,
                Gender = model.Gender,
                PhoneNumber = model.PhoneNumber,
                Status = model.Status,
            };

            _serviceUsers.Insert(newUsers);

            try
            {
                if ((model.RoleId?.Length ?? 0) > 0)
                {
                    UserRoles userRoles = new UserRoles()
                    {
                        RoleId = model.RoleId,
                        UserId = newUsers.Id
                    };
                    _serviceUserRole.Insert(userRoles);
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

        [HttpPost, DisableRequestSizeLimit]
        [Route("uploadfile")]
        public IActionResult UploadFile()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("ClientApp","public","Images", "Avatar");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim().ToString();
                    
                    FileInfo fileInfo = new FileInfo(fileName);
                    string newFileName = System.IO.Path.GetRandomFileName() + fileInfo.Extension;

                    var fullPath = Path.Combine(pathToSave,newFileName);
                    //var dbPath = Path.Combine(folderName, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create ))
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
            var passwordHasher = new PasswordHasher<LoginModel>();
            //var passwordHash = passwordHasher.HashPassword(login, login.Password);
            try
            {
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

        [HttpGet]
        [Route("Detail/{id}")]
        public async Task<UserDetail> GetAccountDetail(
            string id
            )
        {
            try
            {
                var user = await _serviceUsers.FindAsync(id);
                var role = _serviceUserRole.Queryable().Where(x => x.UserId == user.Id).FirstOrDefault();

                var userDetail = user.Adapt<UserDetail>();
                if (role != null) userDetail.RoleId = role.RoleId;
                return userDetail;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get account detail, accountid: {0}", id);
            }
            finally
            {
                _logger.LogInformation("Get account detail, accountId {0}", id);
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
                    var user = await _serviceUsers.FindAsync(model.Id);
                    if (user != null)
                    {
                        user.Address = model.Address;
                        user.Avatar = (model.Avatar?.Length ?? 0) > 0 ? model.Avatar : user.Avatar;
                        user.FullName = model.FullName;
                        user.Gender = model.Gender;
                        user.PhoneNumber = model.PhoneNumber;
                        user.Status = model.Status;

                        _serviceUsers.Update(user);

                        if ((model.RoleId?.Length ?? 0) > 0)
                        {
                            var roleUser = _serviceUserRole.Queryable().Where(x => x.UserId == model.Id).FirstOrDefault();

                            if (roleUser != null)
                            {
                                _serviceUserRole.Delete(roleUser);
                            }

                            roleUser = new UserRoles()
                            {
                                UserId = model.Id,
                                RoleId = model.RoleId
                            };
                            _serviceUserRole.Insert(roleUser);

                            await _unitOfWork.SaveChangesAsync();
                        }

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
                _logger.LogError(ex, "Update user, account: {0}", model);
            }
            finally
            {
                _logger.LogInformation("Update user, account: {0}, Result status: ", model, isActionSuccess);
            }
            return Ok(new { Successful = false, Error = "Has error when update" });
        }

        [HttpPost]
        [Route("load-data")]
        public async Task<IActionResult> LoadData([FromForm] string draw, [FromForm] string start, [FromForm] string length)
        {
            try
            {
                //var draw1 = draw ;// HttpContext.Request.Form["draw"].FirstOrDefault();

                // Skip number of Rows count
                //var start = Request.Form["start"].FirstOrDefault();

                // Paging Length 10,20
                //var length = Request.Form["length"].FirstOrDefault();

                //// Sort Column Name
                //var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();

                //// Sort Column Direction (asc, desc)
                //var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();

                // Search Value from (Search box)
                //var searchValue = Request.Form["search[value]"].FirstOrDefault();

                //Paging Size (10, 20, 50,100)
                int pageSize = length != null ? Convert.ToInt32(length) : 0;

                int skip = start != null ? Convert.ToInt32(start) : 0;
                int pageIndex = skip / pageSize + 1;
                int recordsTotal = 0;

                var rs = await Get(pageSize, pageIndex);

                // getting all Customer data
                //string SearchUserName = HttpContext.Request.Form["SearchUserName"].FirstOrDefault();
                //string SearchEmail = HttpContext.Request.Form["SearchEmail"].FirstOrDefault();
                //string SearchPhone = HttpContext.Request.Form["SearchPhone"].FirstOrDefault();
                //string SearchAccountStatus = HttpContext.Request.Form["SearchAccountStatus"].FirstOrDefault();
                //string SearchDate = HttpContext.Request.Form["SearchDate"].FirstOrDefault();

                //DateTime fromDate = DateTime.ParseExact(SearchDate.Split('~')[0].Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //DateTime toDate = DateTime.ParseExact(SearchDate.Split('~')[1].Trim() + " 23:59:59", "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                //string filter = $"/api/v{API_VERSION}/ManageUser/?pagesize={pageSize}&pageindex={pageIndex}" +
                //    $"&username={SearchUserName}" +
                //    $"&phone={SearchPhone}" +
                //    $"&email={SearchEmail}" +
                //    $"&fdate={fromDate}" +
                //    $"&tdate={toDate}" +
                //    $"&status={SearchAccountStatus}";

                //string apiResult = await Helper.CallRestApiAsync(_shopWalletInsideApiLink + filter);
                //var rs = JsonConvert.DeserializeObject<PaggingModel<Users>>(apiResult);

                //var listUser = _serviceUsers.Queryable().Where(searCondition).OrderByDescending(x => x.CreatedDate);

                //pageSize = pageSize == 0 ? Constants.DEFAULT_PAGE_SIZE : pageSize;
                //var pagedListUser = await PaginatedList<Users>.CreateAsync(listUser, pageIndex ?? 1, pageSize);
                ////var pagedListUser = await PaginatedList<Users>.CreateAsync(listUser,pageSize);
                //PaggingModel<Users> returnResult = new PaggingModel<Users>()
                //{
                //    ListData = pagedListUser.Adapt<List<Users>>(),
                //    TotalRows = pagedListUser.TotalRows,
                //};

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
    }
}