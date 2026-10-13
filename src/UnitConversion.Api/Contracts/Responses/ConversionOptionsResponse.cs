using System.Text.Json.Serialization;

namespace UnitConversion.Api.Contracts.Responses;

public sealed class ConversionOptionsResponse
{
    [JsonPropertyName("options")]
    public IReadOnlyList<ConversionOption> Options { get; init; } = Array.Empty<ConversionOption>();
}

public sealed class ConversionOption
{
    [JsonPropertyName("conversionType")]
    public string ConversionType { get; init; } = string.Empty;

    [JsonPropertyName("units")]
    public IReadOnlyList<string> Units { get; init; } = Array.Empty<string>();
}
