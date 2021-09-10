using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Text.Json.Serialization;

namespace Blazor.Keycloak.Client.Authentication
{
    public class CustomOidcOptions: OidcProviderOptions
    {
        [JsonPropertyName("client_secret")]
        public string ClientSecret { get; set; }
    }
}
