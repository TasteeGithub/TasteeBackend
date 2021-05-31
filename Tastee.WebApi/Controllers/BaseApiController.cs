using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Tastee.WebApi.Controllers
{
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
        public string CurrentUser { get { 
            return  User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
    } }
    }
}
