using UnitConversion.Application.Factories;
using UnitConversion.Application.Interfaces;
using UnitConversion.Application.Validators;
using UnitConversion.Domain.Enums;
using UnitConversion.Domain.Models;

namespace UnitConversion.Application.Services;

public sealed class ConversionService : IConversionService
{
    private readonly IUnitRepository _unitRepository;
    private readonly ConversionStrategyResolver _strategyResolver;
    private readonly ConversionRequestValidator _validator;

    public ConversionService(IUnitRepository unitRepository, ConversionStrategyResolver strategyResolver, ConversionRequestValidator validator)
    {
        _unitRepository = unitRepository;
        _strategyResolver = strategyResolver;
        _validator = validator;
    }

    private const int PublicPrecision = 12;

    public ConversionResult Convert(decimal value, ConversionType conversionType, Unit fromUnit, Unit toUnit)
    {
        _validator.Validate(value, conversionType, fromUnit, toUnit);

        if (fromUnit == toUnit)
        {
            return new ConversionResult(value, conversionType, fromUnit, toUnit, value);
        }

        var strategy = _strategyResolver.Resolve(conversionType);
        var convertedValue = strategy.Convert(value, fromUnit, toUnit);

        if (convertedValue == decimal.MinValue || convertedValue == decimal.MaxValue)
        {
            throw new InvalidOperationException("The conversion result is invalid or exceeds the supported numeric range.");
        }

        var normalizedValue = decimal.Round(convertedValue, PublicPrecision, MidpointRounding.AwayFromZero);
        return new ConversionResult(value, conversionType, fromUnit, toUnit, normalizedValue);
    }

    public IReadOnlyDictionary<ConversionType, IReadOnlyList<Unit>> GetAvailableOptions()
    {
        return _unitRepository.GetUnitsByCategory();
    }
}
