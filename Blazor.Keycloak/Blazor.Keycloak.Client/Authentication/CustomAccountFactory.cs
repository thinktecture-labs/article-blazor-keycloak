using Blazor.Keycloak.Shared.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;

namespace Blazor.Keycloak.Client.Authentication
{
    public class CustomAccountFactory : AccountClaimsPrincipalFactory<RemoteUserAccount>
    {
        private readonly HttpClient _httpClient;

        public CustomAccountFactory(IAccessTokenProviderAccessor accessor, HttpClient httpClient)
            : base(accessor)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async override ValueTask<ClaimsPrincipal> CreateUserAsync(
                    RemoteUserAccount account, RemoteAuthenticationUserOptions options)
        {
            var initialUser = await base.CreateUserAsync(account, options);
            try
            {
                return (initialUser.Identity != null && initialUser.Identity.IsAuthenticated) 
                    ? await KeycloakClaimsHelper.TransformRolesAsync(initialUser, "blazor-keycloak-web-api") 
                    : initialUser;
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode != System.Net.HttpStatusCode.Unauthorized)
                {
                    throw ex;
                }
            }

            return initialUser;
        }
    }
}
