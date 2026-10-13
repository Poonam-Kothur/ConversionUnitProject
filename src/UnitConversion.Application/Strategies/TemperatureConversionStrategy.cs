using UnitConversion.Application.Interfaces;
using UnitConversion.Domain.Enums;
using UnitConversion.Domain.Exceptions;
using UnitConversion.Domain.Models;

namespace UnitConversion.Application.Strategies;

public sealed class TemperatureConversionStrategy : IUnitConversionStrategy
{
    private readonly IUnitRepository _unitRepository;

    public TemperatureConversionStrategy(IUnitRepository unitRepository)
    {
        _unitRepository = unitRepository;
    }

    public ConversionType ConversionType => ConversionType.Temperature;

    public decimal Convert(decimal value, Unit fromUnit, Unit toUnit)
    {
        var fromDefinition = GetDefinition(fromUnit);
        var toDefinition = GetDefinition(toUnit);

        var celsiusValue = (value - fromDefinition.OffsetToBaseUnit) * fromDefinition.FactorToBaseUnit;
        return (celsiusValue / toDefinition.FactorToBaseUnit) + toDefinition.OffsetToBaseUnit;
    }

    private UnitDefinition GetDefinition(Unit unit)
    {
        if (!_unitRepository.TryGetDefinition(unit, out var definition) || definition!.ConversionType != ConversionType.Temperature)
        {
            throw new InvalidUnitException($"Unit '{unit}' is not valid for the '{ConversionType.Temperature}' conversion type.");
        }

        return definition;
    }
}
