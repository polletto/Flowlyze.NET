using System.Net.Http.Json;
using Flowlyze.Models;

namespace Flowlyze;

/// <summary>
/// Client for Flowlyze Flow API operations.
/// </summary>
public sealed class FlowClient
{
    private readonly FlowlyzeClient _client;

    internal FlowClient(FlowlyzeClient client)
    {
        _client = client;
    }

    /// <summary>
    /// Gets a flow by its identifier.
    /// </summary>
    /// <param name="id">Flow identifier.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    public async Task<FlowResponse?> GetByIdAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);

        var escapedId = Uri.EscapeDataString(id);
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/api/flows/{escapedId}");
        using var response = await _client
            .SendAsync(request, cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        response.EnsureSuccessStatusCode();

        return await response.Content
            .ReadFromJsonAsync<FlowResponse>(cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }
}
