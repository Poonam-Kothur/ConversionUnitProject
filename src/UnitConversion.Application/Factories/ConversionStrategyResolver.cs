using UnitConversion.Application.Interfaces;
using UnitConversion.Domain.Enums;
using UnitConversion.Domain.Exceptions;

namespace UnitConversion.Application.Factories;

public sealed class ConversionStrategyResolver
{
    private readonly IReadOnlyDictionary<ConversionType, IUnitConversionStrategy> _strategies;

    public ConversionStrategyResolver(IEnumerable<IUnitConversionStrategy> strategies)
    {
        var strategyLookup = new Dictionary<ConversionType, IUnitConversionStrategy>();

        foreach (var strategy in strategies)
        {
            if (strategyLookup.ContainsKey(strategy.ConversionType))
            {
                throw new InvalidOperationException($"Duplicate strategy registration detected for conversion type '{strategy.ConversionType}'.");
            }

            strategyLookup[strategy.ConversionType] = strategy;
        }

        _strategies = strategyLookup;
    }

    public IUnitConversionStrategy Resolve(ConversionType conversionType)
    {
        if (_strategies.TryGetValue(conversionType, out var strategy))
        {
            return strategy;
        }

        throw new UnsupportedConversionTypeException($"No conversion strategy is registered for '{conversionType}'.");
    }
}
