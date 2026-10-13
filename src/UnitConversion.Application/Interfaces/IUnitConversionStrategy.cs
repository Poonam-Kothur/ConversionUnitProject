using UnitConversion.Domain.Enums;

namespace UnitConversion.Application.Interfaces;

public interface IUnitConversionStrategy
{
    ConversionType ConversionType { get; }
    decimal Convert(decimal value, Unit fromUnit, Unit toUnit);
}
