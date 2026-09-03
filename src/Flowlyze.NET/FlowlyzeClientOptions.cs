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
}
