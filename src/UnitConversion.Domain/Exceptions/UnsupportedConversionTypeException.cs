namespace UnitConversion.Domain.Exceptions;

public sealed class UnsupportedConversionTypeException : Exception
{
    public UnsupportedConversionTypeException(string message)
        : base(message)
    {
    }
}
