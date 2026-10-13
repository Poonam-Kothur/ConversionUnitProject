namespace UnitConversion.Domain.Exceptions;

public sealed class InvalidUnitException : Exception
{
    public InvalidUnitException(string message)
        : base(message)
    {
    }
}
