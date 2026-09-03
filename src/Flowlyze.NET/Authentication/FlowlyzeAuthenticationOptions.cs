namespace Flowlyze.Authentication;

/// <summary>
/// OAuth 2.0 client credentials configuration for the Flowlyze API.
/// </summary>
public sealed class FlowlyzeAuthenticationOptions
{
    public static readonly Uri DefaultTokenEndpoint =
        new("https://secure-oauth-prd.eu.auth0.com/oauth/token");

    public required string ClientId { get; init; }

    public required string ClientSecret { get; init; }

    public required string Audience { get; init; }

    public Uri TokenEndpoint { get; init; } = DefaultTokenEndpoint;

    /// <summary>
    /// Refresh the cached token this long before its reported expiration time.
    /// </summary>
    public TimeSpan RefreshBeforeExpiration { get; init; } = TimeSpan.FromMinutes(1);
}
