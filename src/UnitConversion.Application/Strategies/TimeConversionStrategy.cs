using UnitConversion.Application.Interfaces;
using UnitConversion.Domain.Enums;
using UnitConversion.Domain.Exceptions;
using UnitConversion.Domain.Models;

namespace UnitConversion.Application.Strategies;

public sealed class TimeConversionStrategy : IUnitConversionStrategy
{
    private readonly IUnitRepository _unitRepository;

    public TimeConversionStrategy(IUnitRepository unitRepository)
    {
        _unitRepository = unitRepository;
    }

    public ConversionType ConversionType => ConversionType.Time;

    public decimal Convert(decimal value, Unit fromUnit, Unit toUnit)
    {
        var fromDefinition = GetDefinition(fromUnit);
        var toDefinition = GetDefinition(toUnit);
        return (value * fromDefinition.FactorToBaseUnit) / toDefinition.FactorToBaseUnit;
    }

    private UnitDefinition GetDefinition(Unit unit)
    {
        if (!_unitRepository.TryGetDefinition(unit, out var definition) || definition!.ConversionType != ConversionType.Time)
        {
            throw new InvalidUnitException($"Unit '{unit}' is not valid for the '{ConversionType.Time}' conversion type.");
        }

        return definition;
    }
}
