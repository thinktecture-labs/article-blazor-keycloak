using Blazor.Keycloak.Shared.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace Blazor.Keycloak.Client.Authentication
{
    public class CustomAccountFactory : AccountClaimsPrincipalFactory<RemoteUserAccount>
    {
        private readonly IAccessTokenProviderAccessor _accessor;
        private readonly HttpClient _httpClient;

        public CustomAccountFactory(IAccessTokenProviderAccessor accessor, HttpClient httpClient)
            : base(accessor)
        {
            _accessor = accessor;
            _httpClient = httpClient;
        }

        public async override ValueTask<ClaimsPrincipal> CreateUserAsync(
                    RemoteUserAccount account, RemoteAuthenticationUserOptions options)
        {
            var initialUser = await base.CreateUserAsync(account, options);

            if (initialUser.Identity.IsAuthenticated)
            {
                var accessTokenResult = await _accessor.TokenProvider.RequestAccessToken();
                if (accessTokenResult.TryGetToken(out var accessToken))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.Value);
                }
                var roles = await _httpClient.GetFromJsonAsync<string[]>("User/roles");
                var result = initialUser.Clone();
                if (result.Identity is ClaimsIdentity identity)
                {
                    foreach (var role in roles)
                    {
                        var value = role;
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            identity.AddClaim(new Claim("roles", value));
                        }
                    }
                    return result;
                }
            }

            return initialUser;
        }
    }
}
