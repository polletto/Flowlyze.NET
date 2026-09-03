using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Flowlyze.Authentication;

/// <summary>
/// Retrieves and caches Flowlyze OAuth access tokens using the Auth0 client_credentials flow.
/// </summary>
public sealed class Auth0AccessTokenProvider : IFlowlyzeAccessTokenProvider, IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly FlowlyzeAuthenticationOptions _options;
    private readonly SemaphoreSlim _refreshLock = new(1, 1);

    private string? _accessToken;
    private DateTimeOffset _expiresAt;

    public Auth0AccessTokenProvider(
        HttpClient httpClient,
        FlowlyzeAuthenticationOptions options)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public async ValueTask<string> GetAccessTokenAsync(
        CancellationToken cancellationToken = default)
    {
        if (HasValidToken())
        {
            return _accessToken!;
        }

        await _refreshLock.WaitAsync(cancellationToken).ConfigureAwait(false);

        try
        {
            if (HasValidToken())
            {
                return _accessToken!;
            }

            using var response = await _httpClient.PostAsJsonAsync(
                _options.TokenEndpoint,
                new TokenRequest(
                    "client_credentials",
                    _options.ClientId,
                    _options.ClientSecret,
                    _options.Audience),
                cancellationToken).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            var token = await response.Content
                .ReadFromJsonAsync<TokenResponse>(cancellationToken: cancellationToken)
                .ConfigureAwait(false)
                ?? throw new InvalidOperationException("The OAuth token endpoint returned an empty response.");

            if (string.IsNullOrWhiteSpace(token.AccessToken))
            {
                throw new InvalidOperationException("The OAuth token endpoint did not return an access token.");
            }

            _accessToken = token.AccessToken;
            _expiresAt = DateTimeOffset.UtcNow.AddSeconds(token.ExpiresIn);

            return _accessToken;
        }
        finally
        {
            _refreshLock.Release();
        }
    }

    private bool HasValidToken() =>
        !string.IsNullOrWhiteSpace(_accessToken) &&
        DateTimeOffset.UtcNow < _expiresAt - _options.RefreshBeforeExpiration;

    public void Dispose() => _refreshLock.Dispose();

    private sealed record TokenRequest(
        [property: JsonPropertyName("grant_type")] string GrantType,
        [property: JsonPropertyName("client_id")] string ClientId,
        [property: JsonPropertyName("client_secret")] string ClientSecret,
        [property: JsonPropertyName("audience")] string Audience);

    private sealed record TokenResponse(
        [property: JsonPropertyName("access_token")] string AccessToken,
        [property: JsonPropertyName("token_type")] string TokenType,
        [property: JsonPropertyName("expires_in")] int ExpiresIn);
}
