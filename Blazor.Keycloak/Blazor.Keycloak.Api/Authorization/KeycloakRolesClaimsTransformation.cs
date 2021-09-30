
using Blazor.Keycloak.Shared.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace Blazor.Keycloak.Api.Authorization;

/// <summary>
/// Transforms keycloak roles in the resource_access claim to jwt role claims.
/// </summary>
/// <example>
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
/// <seealso cref="IClaimsTransformation" />
public class KeycloakRolesClaimsTransformation : IClaimsTransformation
{
	private readonly string _roleClaimType;
	private readonly string _audience;

	/// <summary>
	/// Initializes a new instance of the <see cref="KeycloakRolesClaimsTransformation"/> class.
	/// </summary>
	/// <param name="roleClaimType">Type of the role claim.</param>
	/// <param name="audience">The audience.</param>
	public KeycloakRolesClaimsTransformation(string roleClaimType, string audience)
	{
		_roleClaimType = roleClaimType;
		_audience = audience;
	}

	/// <summary>
	/// Provides a central transformation point to change the specified principal.
	/// Note: this will be run on each AuthenticateAsync call, so its safer to
	/// return a new ClaimsPrincipal if your transformation is not idempotent.
	/// </summary>
	/// <param name="principal">The <see cref="T:System.Security.Claims.ClaimsPrincipal" /> to transform.</param>
	/// <returns>
	/// The transformed principal.
	/// </returns>
	public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
	{
		return KeycloakClaimsHelper.TransformRolesAsync(principal, _audience, _roleClaimType);
	}
}
