using UnitConversion.Application.Interfaces;
using UnitConversion.Domain.Enums;
using UnitConversion.Domain.Exceptions;
using UnitConversion.Domain.Models;

namespace UnitConversion.Application.Strategies;

public sealed class WeightConversionStrategy : IUnitConversionStrategy
{
    private readonly IUnitRepository _unitRepository;

    public WeightConversionStrategy(IUnitRepository unitRepository)
    {
        _unitRepository = unitRepository;
    }

    public ConversionType ConversionType => ConversionType.Weight;

    public decimal Convert(decimal value, Unit fromUnit, Unit toUnit)
    {
        var fromDefinition = GetDefinition(fromUnit);
        var toDefinition = GetDefinition(toUnit);
        return (value * fromDefinition.FactorToBaseUnit) / toDefinition.FactorToBaseUnit;
    }

    private UnitDefinition GetDefinition(Unit unit)
    {
        if (!_unitRepository.TryGetDefinition(unit, out var definition) || definition!.ConversionType != ConversionType.Weight)
        {
            throw new InvalidUnitException($"Unit '{unit}' is not valid for the '{ConversionType.Weight}' conversion type.");
        }

        return definition;
    }
}
