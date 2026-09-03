namespace Flowlyze.Authentication;

/// <summary>
/// OAuth 2.0 client-credentials configuration for the Flowlyze API.
/// </summary>
public sealed class OAuthAuthenticationOptions
{
    public static readonly Uri DefaultTokenEndpoint =
        new("https://secure-oauth-prd.eu.auth0.com/oauth/token");

    public required string ClientId { get; init; }
    public required string ClientSecret { get; init; }
    public required string Audience { get; init; }

    public Uri TokenEndpoint { get; init; } = DefaultTokenEndpoint;

    /// <summary>
    /// Optional tenant identifier. The current general API documentation describes the
    /// OAuth tenant header as tenant_id; this remains isolated here while the definitive
    /// authentication contract is being confirmed.
    /// </summary>
    public string? TenantId { get; init; }

    /// <summary>
    /// Refresh the cached token this long before its reported expiration time.
    /// </summary>
    public TimeSpan RefreshBeforeExpiration { get; init; } = TimeSpan.FromMinutes(1);
}
