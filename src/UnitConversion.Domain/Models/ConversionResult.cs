using UnitConversion.Domain.Enums;

namespace UnitConversion.Domain.Models;

public sealed record ConversionResult(
    decimal OriginalValue,
    ConversionType ConversionType,
    Unit FromUnit,
    Unit ToUnit,
    decimal ConvertedValue);
