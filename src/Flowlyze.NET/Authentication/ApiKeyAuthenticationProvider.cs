namespace Flowlyze.Authentication;

/// <summary>
/// Applies Flowlyze API-key authentication headers.
/// </summary>
public sealed class ApiKeyAuthenticationProvider : IFlowlyzeAuthenticationProvider
{
    private const string ApiKeyHeaderName = "x-api-key";
    private const string TenantHeaderName = "x-tenant-id";

    private readonly ApiKeyAuthenticationOptions _options;

    public ApiKeyAuthenticationProvider(ApiKeyAuthenticationOptions options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public ValueTask ApplyAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        request.Headers.Remove(ApiKeyHeaderName);
        request.Headers.TryAddWithoutValidation(ApiKeyHeaderName, _options.ApiKey);

        request.Headers.Remove(TenantHeaderName);
        if (!string.IsNullOrWhiteSpace(_options.TenantId))
        {
            request.Headers.TryAddWithoutValidation(TenantHeaderName, _options.TenantId);
        }

        return ValueTask.CompletedTask;
    }
}
