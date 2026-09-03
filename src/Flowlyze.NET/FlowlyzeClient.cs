using System.Net.Http.Headers;
using Flowlyze.Authentication;

namespace Flowlyze;

/// <summary>
/// Low-level HTTP client for the Flowlyze API.
/// </summary>
public sealed class FlowlyzeClient
{
    private const string TenantHeaderName = "tenant_id";

    private readonly HttpClient _httpClient;
    private readonly IFlowlyzeAccessTokenProvider _accessTokenProvider;
    private readonly FlowlyzeClientOptions _options;

    public FlowlyzeClient(
        HttpClient httpClient,
        IFlowlyzeAccessTokenProvider accessTokenProvider,
        FlowlyzeClientOptions options)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _accessTokenProvider = accessTokenProvider ?? throw new ArgumentNullException(nameof(accessTokenProvider));
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    /// <summary>
    /// Sends an authenticated request to Flowlyze.
    /// Relative request URIs are resolved against <see cref="FlowlyzeClientOptions.BaseAddress"/>.
    /// </summary>
    public async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.RequestUri is null)
        {
            throw new ArgumentException("The request must contain a request URI.", nameof(request));
        }

        if (!request.RequestUri.IsAbsoluteUri)
        {
            request.RequestUri = new Uri(_options.BaseAddress, request.RequestUri);
        }

        var accessToken = await _accessTokenProvider
            .GetAccessTokenAsync(cancellationToken)
            .ConfigureAwait(false);

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        request.Headers.Remove(TenantHeaderName);
        request.Headers.TryAddWithoutValidation(TenantHeaderName, _options.TenantId);

        return await _httpClient
            .SendAsync(request, completionOption, cancellationToken)
            .ConfigureAwait(false);
    }
}
