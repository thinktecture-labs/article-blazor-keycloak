
using Blazor.Keycloak.Client.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;

namespace Blazor.Keycloak.Client.Utils
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds support for Auth0 authentication for SPA applications using <see cref="Auth0ProviderOptions"/> and the <see cref="RemoteAuthenticationState"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="configure">An action that will configure the <see cref="RemoteAuthenticationOptions{TProviderOptions}"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/> where the services were registered.</returns>
        public static IRemoteAuthenticationBuilder<RemoteAuthenticationState, RemoteUserAccount> AddCustomAuthentication(this IServiceCollection services, Action<RemoteAuthenticationOptions<CustomOidcOptions>> configure)
        {
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<RemoteAuthenticationOptions<CustomOidcOptions>>, DefaultCustomProviderOptionsConfiguration>());

            return services.AddRemoteAuthentication<RemoteAuthenticationState, RemoteUserAccount, CustomOidcOptions>(configure);
        }
    }
}