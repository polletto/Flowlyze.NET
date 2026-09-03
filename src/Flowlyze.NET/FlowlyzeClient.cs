namespace Flowlyze;

/// <summary>
/// Low-level HTTP client for the Flowlyze API.
/// </summary>
public sealed class FlowlyzeClient
{
    private const string ApiKeyHeaderName = "x-api-key";
    private const string TenantHeaderName = "x-tenant-id";

    private readonly HttpClient _httpClient;
    private readonly FlowlyzeClientOptions _options;

    public FlowlyzeClient(
        HttpClient httpClient,
        FlowlyzeClientOptions options)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _options = options ?? throw new ArgumentNullException(nameof(options));
        Flows = new FlowClient(this);
    }

    /// <summary>
    /// Strongly typed Flow API operations.
    /// </summary>
    public FlowClient Flows { get; }

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

        request.Headers.Remove(ApiKeyHeaderName);
        request.Headers.TryAddWithoutValidation(ApiKeyHeaderName, _options.ApiKey);

        request.Headers.Remove(TenantHeaderName);
        if (!string.IsNullOrWhiteSpace(_options.TenantId))
        {
            request.Headers.TryAddWithoutValidation(TenantHeaderName, _options.TenantId);
        }

        return await _httpClient
            .SendAsync(request, completionOption, cancellationToken)
            .ConfigureAwait(false);
    }
}
