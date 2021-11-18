
using Blazor.Keycloak.Client.Services;
using Blazor.Keycloak.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blazor.Keycloak.Client.Pages
{
    public partial class Index
    {
        [Inject] private NavigationManager Navigation { get; set; }
        [Inject] private IAccessTokenProvider AccessTokenProvider { get; set; }


        protected override async Task OnInitializedAsync()
        {
            var token = await AccessTokenProvider.RequestAccessToken();
            if (token.Status == AccessTokenResultStatus.Success && token.TryGetToken(out var accessToken))
            {
                if (!String.IsNullOrWhiteSpace(accessToken.Value) || accessToken.Expires > DateTimeOffset.Now)
                {
                    Navigation.NavigateTo("conferences");
                }
            }
            await base.OnInitializedAsync();
        }


        private void BeginSignIn()
        {
            Navigation.NavigateTo("authentication/login");
        }
    }
}