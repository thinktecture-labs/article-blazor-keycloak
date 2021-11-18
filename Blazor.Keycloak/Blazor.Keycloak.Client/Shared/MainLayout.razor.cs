
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

        private bool collapsed = true;
        private string[] selectedKeys = new[] { "conferences" };

        private async Task BeginSignOut()
        {
            await SignOutManager.SetSignOutState();

            Navigation.NavigateTo("authentication/logout");
        }

        private void toggle()
        {
            collapsed = !collapsed;
        }

        private string GetInitials(string name)
        {
            var nameSplit = name.Split(new string[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries);

            var initials = "";

            foreach (string item in nameSplit)
            {
                initials += item.Substring(0, 1).ToUpper();
            }

            return initials;
        }

        private void SelectedKeyChanged(string[] keys)
        {
            if (selectedKeys.Except(keys).Count() > 0)
            {
                var navigationKey = keys.FirstOrDefault();
                Console.WriteLine(navigationKey);
                if (!String.IsNullOrWhiteSpace(navigationKey))
                {
                    Navigation.NavigateTo(navigationKey);
                    selectedKeys = keys;
                }
            }
        }
    }
}