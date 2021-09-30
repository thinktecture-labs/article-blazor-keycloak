
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.Keycloak.Client.Shared
{
    public partial class MainLayout
    {
        [Inject] private NavigationManager Navigation { get; set; }
        [Inject] private SignOutSessionStateManager SignOutManager { get; set; }

        private bool collapsed;
        private string[] selectedKeys = new[] { "index" };

        private async Task BeginSignOut()
        {
            await SignOutManager.SetSignOutState();

            Navigation.NavigateTo("authentication/logout");
        }

        private void toggle()
        {
            collapsed = !collapsed;
        }

        private void BeginSignIn()
        {
            Navigation.NavigateTo("authentication/login");
        }

        private void SelectedKeyChanged(string[] keys)
        {
            if (selectedKeys.Except(keys).Count() > 0)
            {
                var navigationKey = keys.FirstOrDefault();
                if (!String.IsNullOrWhiteSpace(navigationKey))
                {
                    Navigation.NavigateTo(navigationKey);
                    selectedKeys = keys;
                }
            }
        }
    }
}