using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Tastee.Application.Features.Menus.Commands;
using Tastee.Shared;

namespace Tastee.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : BaseApiController
    {
        private readonly ILogger<MenuController> _logger;

        public MenuController(ILogger<MenuController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Thêm mới Menu
        /// </summary>
        /// <param name="createMenuCommand"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(CreateMenuCommand createMenuCommand)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    createMenuCommand.CreatedBy = CurrentUser;
                    return Ok(await Mediator.Send(createMenuCommand));
                }
                else
                {
                    var errorMessage = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage);
                    return Ok(new Response { Successful = false, Message = string.Join(",", errorMessage) });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Insert menu, Menu: {0}", createMenuCommand);
            }
            finally
            {
                _logger.LogInformation("Insert menu, Menu: {0}", createMenuCommand);
            }
            return Ok(new { Successful = false, Error = "Has error when insert menu" });
        }

        /// <summary>
        /// Cập nhật menu
        /// </summary>
        /// <param name="updateMenuCommand"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update(UpdateMenuCommand updateMenuCommand)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    updateMenuCommand.UpdateBy = CurrentUser;
                    return Ok(Mediator.Send(updateMenuCommand));
                }
                else
                {
                    var errorMessage = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage);
                    return Ok(new Response { Successful = false, Message = string.Join(",", errorMessage) });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update Menu, Menu: {0}", updateMenuCommand);
            }
            finally
            {
                _logger.LogInformation("Update Menu, Menu: {0}", updateMenuCommand);
            }
            return Ok(new { Successful = false, Error = "Has error when update Menu" });
        }
    }
}