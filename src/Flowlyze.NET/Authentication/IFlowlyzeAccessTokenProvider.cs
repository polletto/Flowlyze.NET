namespace Flowlyze.Authentication;

/// <summary>
/// Provides OAuth access tokens used to authenticate Flowlyze API requests.
/// </summary>
public interface IFlowlyzeAccessTokenProvider
{
    ValueTask<string> GetAccessTokenAsync(CancellationToken cancellationToken = default);
}
