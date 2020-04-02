using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using TTBackEnd.Shared;

//using TTFrontEnd.Models.SqlDataContext;
using LoginModel = TTBackEnd.Shared.LoginModel;

namespace TTFrontEnd.Controllers
{
    public class AuthenController : Controller
    {
        private readonly ILogger<AuthenController> _logger;
        private readonly HttpClient _httpClient;

        public AuthenController(
            ILogger<AuthenController> logger,
            IHttpClientFactory httpClientFactory
         )
        {
            _httpClient = httpClientFactory.CreateClient("LoginClient");
            _logger = logger;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl = "")
        {
            try
            {
                if (ModelState.IsValid)
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
                        _logger.LogInformation("Logging: Email:{0}", model.Email);
                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                            return Redirect(returnUrl);
                        else
                            return RedirectToAction("Index", "Home");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error:{0}", ex.ToString());
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