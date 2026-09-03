namespace Flowlyze.Authentication;

/// <summary>
/// Applies authentication information to an outgoing Flowlyze API request.
/// </summary>
public interface IFlowlyzeAuthenticationProvider
{
    ValueTask ApplyAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken = default);
}
