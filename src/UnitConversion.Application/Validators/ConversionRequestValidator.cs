using UnitConversion.Application.Interfaces;
using UnitConversion.Domain.Enums;
using UnitConversion.Domain.Exceptions;

namespace UnitConversion.Application.Validators;

public sealed class ConversionRequestValidator
{
    private readonly IUnitRepository _unitRepository;

    public ConversionRequestValidator(IUnitRepository unitRepository)
    {
        _unitRepository = unitRepository;
    }

    public void Validate(decimal value, ConversionType conversionType, Unit fromUnit, Unit toUnit)
    {
        if (!Enum.IsDefined(typeof(ConversionType), conversionType))
        {
            throw new UnsupportedConversionTypeException($"Unsupported conversion type '{conversionType}'.");
        }

        if (!_unitRepository.TryGetDefinition(fromUnit, out var fromDefinition) || !_unitRepository.TryGetDefinition(toUnit, out var toDefinition))
        {
            throw new InvalidUnitException("The supplied unit is not recognized or is not supported.");
        }

        if (fromDefinition!.ConversionType != conversionType || toDefinition!.ConversionType != conversionType)
        {
            throw new InvalidUnitException($"Units '{fromUnit}' and '{toUnit}' are not valid for conversion type '{conversionType}'.");
        }

        if (value == decimal.MinValue || value == decimal.MaxValue)
        {
            throw new InvalidOperationException("The numeric value is outside the supported conversion range.");
        }
    }
}
