namespace Flowlyze;

/// <summary>
/// Configuration used by <see cref="FlowlyzeClient"/>.
/// </summary>
public sealed class FlowlyzeClientOptions
{
    /// <summary>
    /// Base address of the Flowlyze API.
    /// </summary>
    public required Uri BaseAddress { get; init; }

    /// <summary>
    /// API key sent through the x-api-key header.
    /// </summary>
    public required string ApiKey { get; init; }

    /// <summary>
    /// Optional tenant identifier sent through the x-tenant-id header.
    /// Required when using a multi-tenant authentication context, such as an admin API key.
    /// </summary>
    public string? TenantId { get; init; }
}
