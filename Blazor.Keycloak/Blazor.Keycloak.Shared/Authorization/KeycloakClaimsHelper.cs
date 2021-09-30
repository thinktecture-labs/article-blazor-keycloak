
using Microsoft.AspNetCore.Authentication;
using System;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace Blazor.Keycloak.Shared.Authorization
{

	public static class KeycloakClaimsHelper
	{

		/// <summary>
		/// Transforms keycloak roles in the resource_access claim to jwt role claims.
		/// </summary>
		/// <param name="principal">The <see cref="T:System.Security.Claims.ClaimsPrincipal" /> to transform.</param>
		/// /// <example>
		/// Example of keycloak resource_access claim
		/// "resource_access": {
		///     "blog-api": {
		///         "roles": [
		///             "editor"
		///         ]
		///     },
		///     "account": {
		///         "roles": [
		///             "view-profile"
		///         ]
		///     }
		/// }.
		/// </example>
		/// <returns>
		/// The transformed principal.
		/// </returns>
		public static Task<ClaimsPrincipal> TransformRolesAsync(ClaimsPrincipal principal, string audience, string roleClaimType = "roles")
		{
			var result = principal.Clone();
			if (result.Identity is not ClaimsIdentity identity)
			{
				return Task.FromResult(result);
			}

			var resourceAccessValue = principal.FindFirst("resource_access")?.Value;
			if (String.IsNullOrWhiteSpace(resourceAccessValue))
			{
				return Task.FromResult(result);
			}

			using var resourceAccess = JsonDocument.Parse(resourceAccessValue);
			var clientRoles = resourceAccess
				.RootElement
				.GetProperty(audience)
				.GetProperty("roles");

			foreach (var role in clientRoles.EnumerateArray())
			{
				var value = role.GetString();
				if (!String.IsNullOrWhiteSpace(value))
				{
					identity.AddClaim(new Claim(roleClaimType, value));
				}
			}

			return Task.FromResult(result);
		}
	}
}