using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Security.Claims;

namespace Tastee.WebApi.Controllers
{
    /// <summary>
    /// Controller chứa một số phương thức chung cho các controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        private IMediator _mediator;

        /// <summary>
        ///
        /// </summary>
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        /// <summary>
        /// User hiện tại đang đăng nhập
        /// </summary>
        public string CurrentUser
        {
            get
            {
                return User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            }
        }
        /// <summary>
        /// Chuyển object thành chuỗi json
        /// </summary>
        /// <param name="obj">đối tượng cần chuyển sang json</param>
        /// <returns>Chuỗi json</returns>
        protected string ObjectToJson(object obj) => obj is null ? string.Empty : System.Text.Json.JsonSerializer.Serialize(obj);
    }
}