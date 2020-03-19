using Microsoft.AspNetCore.Components;

namespace TTBackEnd.Client
{
    public class RedirectToLogin : ComponentBase
    {
        [Inject]
        public NavigationManager Navigation { get; set; }

        protected override void OnInitialized()
        {
            Navigation.NavigateTo("login");
        }
    }
}