using System.Text.Json.Serialization;

namespace UnitConversion.Api.Contracts.Requests;

public sealed class ConversionRequest
{
    [JsonPropertyName("value")]
    public decimal? Value { get; set; }

    [JsonPropertyName("conversionType")]
    public string? ConversionType { get; set; }

    [JsonPropertyName("fromUnit")]
    public string? FromUnit { get; set; }

    [JsonPropertyName("toUnit")]
    public string? ToUnit { get; set; }
}
