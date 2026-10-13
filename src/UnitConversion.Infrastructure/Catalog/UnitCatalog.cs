using UnitConversion.Domain.Enums;
using UnitConversion.Domain.Models;

namespace UnitConversion.Infrastructure.Catalog;

public static class UnitCatalog
{
    public static IReadOnlyDictionary<Unit, UnitDefinition> Definitions { get; } = new Dictionary<Unit, UnitDefinition>
    {
        [Unit.Millimeter] = new UnitDefinition(Unit.Millimeter, nameof(Unit.Millimeter), ConversionType.Length, 0.001m),
        [Unit.Centimeter] = new UnitDefinition(Unit.Centimeter, nameof(Unit.Centimeter), ConversionType.Length, 0.01m),
        [Unit.Meter] = new UnitDefinition(Unit.Meter, nameof(Unit.Meter), ConversionType.Length, 1m),
        [Unit.Kilometer] = new UnitDefinition(Unit.Kilometer, nameof(Unit.Kilometer), ConversionType.Length, 1000m),
        [Unit.Inch] = new UnitDefinition(Unit.Inch, nameof(Unit.Inch), ConversionType.Length, 0.0254m),
        [Unit.Foot] = new UnitDefinition(Unit.Foot, nameof(Unit.Foot), ConversionType.Length, 0.3048m),
        [Unit.Yard] = new UnitDefinition(Unit.Yard, nameof(Unit.Yard), ConversionType.Length, 0.9144m),
        [Unit.Mile] = new UnitDefinition(Unit.Mile, nameof(Unit.Mile), ConversionType.Length, 1609.344m),

        [Unit.Milligram] = new UnitDefinition(Unit.Milligram, nameof(Unit.Milligram), ConversionType.Weight, 0.000001m),
        [Unit.Gram] = new UnitDefinition(Unit.Gram, nameof(Unit.Gram), ConversionType.Weight, 0.001m),
        [Unit.Kilogram] = new UnitDefinition(Unit.Kilogram, nameof(Unit.Kilogram), ConversionType.Weight, 1m),
        [Unit.MetricTon] = new UnitDefinition(Unit.MetricTon, nameof(Unit.MetricTon), ConversionType.Weight, 1000m),
        [Unit.Ounce] = new UnitDefinition(Unit.Ounce, nameof(Unit.Ounce), ConversionType.Weight, 0.028349523125m),
        [Unit.Pound] = new UnitDefinition(Unit.Pound, nameof(Unit.Pound), ConversionType.Weight, 0.45359237m),

        [Unit.Celsius] = new UnitDefinition(Unit.Celsius, nameof(Unit.Celsius), ConversionType.Temperature, 1m, 0m),
        [Unit.Fahrenheit] = new UnitDefinition(Unit.Fahrenheit, nameof(Unit.Fahrenheit), ConversionType.Temperature, 5m / 9m, 32m),
        [Unit.Kelvin] = new UnitDefinition(Unit.Kelvin, nameof(Unit.Kelvin), ConversionType.Temperature, 1m, 273.15m),

        [Unit.MetersPerSecond] = new UnitDefinition(Unit.MetersPerSecond, nameof(Unit.MetersPerSecond), ConversionType.Speed, 1m),
        [Unit.KilometersPerHour] = new UnitDefinition(Unit.KilometersPerHour, nameof(Unit.KilometersPerHour), ConversionType.Speed, 0.2777777777777777777777777778m),
        [Unit.MilesPerHour] = new UnitDefinition(Unit.MilesPerHour, nameof(Unit.MilesPerHour), ConversionType.Speed, 0.44704m),
        [Unit.FeetPerSecond] = new UnitDefinition(Unit.FeetPerSecond, nameof(Unit.FeetPerSecond), ConversionType.Speed, 0.3048m),

        [Unit.Millisecond] = new UnitDefinition(Unit.Millisecond, nameof(Unit.Millisecond), ConversionType.Time, 0.001m),
        [Unit.Second] = new UnitDefinition(Unit.Second, nameof(Unit.Second), ConversionType.Time, 1m),
        [Unit.Minute] = new UnitDefinition(Unit.Minute, nameof(Unit.Minute), ConversionType.Time, 60m),
        [Unit.Hour] = new UnitDefinition(Unit.Hour, nameof(Unit.Hour), ConversionType.Time, 3600m),
        [Unit.Day] = new UnitDefinition(Unit.Day, nameof(Unit.Day), ConversionType.Time, 86400m),
        [Unit.Week] = new UnitDefinition(Unit.Week, nameof(Unit.Week), ConversionType.Time, 604800m),

        [Unit.Hertz] = new UnitDefinition(Unit.Hertz, nameof(Unit.Hertz), ConversionType.Frequency, 1m),
        [Unit.Kilohertz] = new UnitDefinition(Unit.Kilohertz, nameof(Unit.Kilohertz), ConversionType.Frequency, 1000m),
        [Unit.Megahertz] = new UnitDefinition(Unit.Megahertz, nameof(Unit.Megahertz), ConversionType.Frequency, 1000000m),
        [Unit.Gigahertz] = new UnitDefinition(Unit.Gigahertz, nameof(Unit.Gigahertz), ConversionType.Frequency, 1000000000m)
    };

    public static IReadOnlyDictionary<ConversionType, IReadOnlyList<Unit>> GetUnitsByCategory()
    {
        return Enum.GetValues<ConversionType>()
            .ToDictionary(
                conversionType => conversionType,
                conversionType => Definitions
                    .Where(pair => pair.Value.ConversionType == conversionType)
                    .Select(pair => pair.Key)
                    .OrderBy(unit => unit)
                    .ToArray() as IReadOnlyList<Unit>);
    }
}
