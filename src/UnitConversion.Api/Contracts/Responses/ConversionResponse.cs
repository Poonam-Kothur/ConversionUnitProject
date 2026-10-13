using System.Text.Json.Serialization;

namespace UnitConversion.Api.Contracts.Responses;

public sealed class ConversionResponse
{
    [JsonPropertyName("originalValue")]
    public decimal OriginalValue { get; init; }

    [JsonPropertyName("conversionType")]
    public string ConversionType { get; init; } = string.Empty;

    [JsonPropertyName("fromUnit")]
    public string FromUnit { get; init; } = string.Empty;

    [JsonPropertyName("toUnit")]
    public string ToUnit { get; init; } = string.Empty;

    [JsonPropertyName("convertedValue")]
    public decimal ConvertedValue { get; init; }
}
