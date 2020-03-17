using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
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
            var tt = await _httpClient.PostAsJsonAsync<RegisterModel>($"{BACKEND_DOMAIN}api/accounts", registerModel);
            //var result = await _httpClient.PostJsonAsync<RegisterResult>("api/accounts", registerModel);

            return new RegisterResult()
            {
                Successful = tt.StatusCode == System.Net.HttpStatusCode.OK
            };
        }

        public async Task<LoginResult> Login(LoginModel loginModel)
        {
            var result = await _httpClient.PostAsJsonAsync<LoginModel>($"{BACKEND_DOMAIN}api/Login", loginModel);

            return new LoginResult()
            {
                Successful = result.StatusCode == System.Net.HttpStatusCode.OK,
                Token = ""//JsonConvert.Deserialize result.Content.ReadAsStringAsync().Result
            };
            //if (result.Successful)
            //{
            //    await _localStorage.SetItemAsync("authToken", result.Token);
            //    ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(result.Token);
            //    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);

            //    return result;
            //}
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}