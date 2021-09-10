using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazor.Keycloak.Client.Authentication;
using Blazor.Keycloak.Client.Utils;
using Blazor.Keycloak.Client.Services;
using Microsoft.AspNetCore.Authentication;
using Blazor.Keycloak.Shared.Authorization;

namespace Blazor.Keycloak.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(typeof(AccountClaimsPrincipalFactory<RemoteUserAccount>), typeof(CustomAccountFactory));
            builder.Services.AddScoped<CustomAuthorizationHeaderHandler>();
            builder.Services.AddScoped<DataService>();
            builder.Services.AddHttpClient("WebAPI",
                    client => client.BaseAddress = new Uri("http://localhost:5002/"))
                .AddHttpMessageHandler<CustomAuthorizationHeaderHandler>();
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
                .CreateClient("WebAPI"));


            builder.Services.AddCustomAuthentication(options =>
            {
                builder.Configuration.Bind("Oidc", options.ProviderOptions);
                options.UserOptions.RoleClaim = "roles";
            });

            builder.Services.AddApiAuthorization();
            builder.Services.AddAntDesign();
            var host = builder.Build();
            await host.RunAsync();
        }
    }
}