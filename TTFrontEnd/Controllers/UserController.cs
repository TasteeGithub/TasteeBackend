using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TTBackEnd.Shared;
using TTFrontEnd.Models.SqlDataContext;
using TTFrontEnd.Services;
using LoginModel = TTBackEnd.Shared.LoginModel;

namespace TTFrontEnd.Controllers
{
    public class UserController : Controller
    {

        private readonly HttpClient _httpClient;
        private readonly ITTService<Users> _userService;
        public UserController(
            ITTService<Users> userService,
            IHttpClientFactory httpClientFactory
         )
        {
            _userService = userService;
            //_httpClientFactory = httpClientFactory;
            _httpClient = httpClientFactory.CreateClient("LoginClient");
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl = "")
        {
            if(ModelState.IsValid)
            {
                var responseResult = await _httpClient.PostAsJsonAsync<LoginModel>($"api/AccountsApi/Login", model);
                var result = JsonConvert.DeserializeObject<LoginResult>(responseResult.Content.ReadAsStringAsync().Result);
                
                if (result.Successful)
                {
                    var claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.Name, model.Email));
                    var claimIdenties = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimPrincipal = new ClaimsPrincipal(claimIdenties);

                    var auManager = Request.HttpContext;
                    await auManager.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal, new AuthenticationProperties()
                    {
                        IsPersistent = model.RememberMe
                    });

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    else
                        return RedirectToAction("Index", "Home");
                }
            }    
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(scheme: CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }

    }
}