using UnitConversion.Domain.Enums;
using UnitConversion.Domain.Models;

namespace UnitConversion.Application.Interfaces;

public interface IConversionService
{
    ConversionResult Convert(decimal value, ConversionType conversionType, Unit fromUnit, Unit toUnit);
    IReadOnlyDictionary<ConversionType, IReadOnlyList<Unit>> GetAvailableOptions();
}
