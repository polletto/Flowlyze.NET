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
    /// Flowlyze tenant identifier sent through the tenant_id header.
    /// </summary>
    public required string TenantId { get; init; }
}
