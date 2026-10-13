using UnitConversion.Domain.Enums;

namespace UnitConversion.Domain.Models;

public sealed record UnitDefinition(
    Unit Unit,
    string DisplayName,
    ConversionType ConversionType,
    decimal FactorToBaseUnit,
    decimal OffsetToBaseUnit = 0m);
