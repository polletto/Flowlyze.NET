using System.Text.Json;
using System.Text.Json.Serialization;

namespace Flowlyze.Models;

/// <summary>
/// Response returned by Flow - GetById.
/// The nested payload and metadata objects remain flexible until their documented schemas are mapped.
/// </summary>
public sealed class FlowResponse
{
    [JsonPropertyName("payload")]
    public JsonElement? Payload { get; init; }

    [JsonPropertyName("metadata")]
    public JsonElement? Metadata { get; init; }
}
