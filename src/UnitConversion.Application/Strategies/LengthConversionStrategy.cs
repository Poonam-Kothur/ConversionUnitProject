using UnitConversion.Application.Interfaces;
using UnitConversion.Domain.Enums;
using UnitConversion.Domain.Exceptions;
using UnitConversion.Domain.Models;

namespace UnitConversion.Application.Strategies;

public sealed class LengthConversionStrategy : IUnitConversionStrategy
{
    private readonly IUnitRepository _unitRepository;

    public LengthConversionStrategy(IUnitRepository unitRepository)
    {
        _unitRepository = unitRepository;
    }

    public ConversionType ConversionType => ConversionType.Length;

    public decimal Convert(decimal value, Unit fromUnit, Unit toUnit)
    {
        var fromDefinition = GetDefinition(fromUnit);
        var toDefinition = GetDefinition(toUnit);
        return (value * fromDefinition.FactorToBaseUnit) / toDefinition.FactorToBaseUnit;
    }

    private UnitDefinition GetDefinition(Unit unit)
    {
        if (!_unitRepository.TryGetDefinition(unit, out var definition) || definition!.ConversionType != ConversionType.Length)
        {
            throw new InvalidUnitException($"Unit '{unit}' is not valid for the '{ConversionType.Length}' conversion type.");
        }

        return definition;
    }
}
