namespace Flowlyze.Authentication;

/// <summary>
/// API-key authentication configuration for the Flowlyze API.
/// </summary>
public sealed class ApiKeyAuthenticationOptions
{
    /// <summary>
    /// API key sent through the x-api-key header.
    /// </summary>
    public required string ApiKey { get; init; }

    /// <summary>
    /// Optional tenant identifier sent through the x-tenant-id header.
    /// </summary>
    public string? TenantId { get; init; }
}
