using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TTBackEnd.Shared;

namespace TTBackEnd.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorage;
        private const string BACKEND_DOMAIN = "https://localhost:44313/";

        public AuthService(HttpClient httpClient,
                           AuthenticationStateProvider authenticationStateProvider,
                           ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            _localStorage = localStorage;
        }

        public async Task<RegisterResult> Register(RegisterModel registerModel)
        {
            var responseResult = await _httpClient.PostAsJsonAsync<RegisterModel>($"{BACKEND_DOMAIN}api/accounts", registerModel);
            var result = JsonConvert.DeserializeObject<RegisterResult>(responseResult.Content.ReadAsStringAsync().Result);
            return result;
        }

        public async Task<LoginResult> Login(LoginModel loginModel)
        {
            var responseResult = await _httpClient.PostAsJsonAsync<LoginModel>($"{BACKEND_DOMAIN}api/Login", loginModel);

            var result = JsonConvert.DeserializeObject<LoginResult>(responseResult.Content.ReadAsStringAsync().Result);

            if (result.Successful)
            {
                await _localStorage.SetItemAsync("authToken", result.Token);
                ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(loginModel.Email);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);

                return result;
            }
            return result;

        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}