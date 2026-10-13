using FluentAssertions;
using UnitConversion.Application.Factories;
using UnitConversion.Application.Interfaces;
using UnitConversion.Application.Services;
using UnitConversion.Application.Strategies;
using UnitConversion.Application.Validators;
using UnitConversion.Domain.Enums;
using UnitConversion.Domain.Exceptions;
using UnitConversion.Infrastructure.Repositories;

namespace UnitConversion.UnitTests;

public class ConversionServiceTests
{
    private readonly IUnitRepository _unitRepository = new UnitRepository();
    private readonly ConversionStrategyResolver _resolver;
    private readonly ConversionRequestValidator _validator;
    private readonly ConversionService _service;

    public ConversionServiceTests()
    {
        _resolver = new ConversionStrategyResolver(new IUnitConversionStrategy[]
        {
            new LengthConversionStrategy(_unitRepository),
            new WeightConversionStrategy(_unitRepository),
            new TemperatureConversionStrategy(_unitRepository),
            new SpeedConversionStrategy(_unitRepository),
            new TimeConversionStrategy(_unitRepository),
            new FrequencyConversionStrategy(_unitRepository),
        });

        _validator = new ConversionRequestValidator(_unitRepository);
        _service = new ConversionService(_unitRepository, _resolver, _validator);
    }

    [Fact]
    public void Convert_WhenSourceIsMeterAndTargetIsKilometer_ReturnsExpectedValue()
    {
        var result = _service.Convert(100m, ConversionType.Length, Unit.Meter, Unit.Kilometer);

        result.ConvertedValue.Should().Be(0.1m);
    }

    [Fact]
    public void Convert_WhenSourceIsKilometerAndTargetIsMeter_ReturnsExpectedValue()
    {
        var result = _service.Convert(2m, ConversionType.Length, Unit.Kilometer, Unit.Meter);

        result.ConvertedValue.Should().Be(2000m);
    }

    [Fact]
    public void Convert_WhenSourceIsCentimeterAndTargetIsInch_ReturnsExpectedValue()
    {
        var result = _service.Convert(10m, ConversionType.Length, Unit.Centimeter, Unit.Inch);

        result.ConvertedValue.Should().BeApproximately(3.937007874015748031496062992m, 0.0000000001m);
    }

    [Fact]
    public void Convert_WhenSourceIsKilogramAndTargetIsGram_ReturnsExpectedValue()
    {
        var result = _service.Convert(2m, ConversionType.Weight, Unit.Kilogram, Unit.Gram);

        result.ConvertedValue.Should().Be(2000m);
    }

    [Fact]
    public void Convert_WhenSourceIsPoundAndTargetIsKilogram_ReturnsExpectedValue()
    {
        var result = _service.Convert(10m, ConversionType.Weight, Unit.Pound, Unit.Kilogram);

        result.ConvertedValue.Should().BeApproximately(4.5359237m, 0.0000001m);
    }

    [Fact]
    public void Convert_WhenSourceIsCelsiusAndTargetIsFahrenheit_ReturnsExpectedValue()
    {
        var result = _service.Convert(25m, ConversionType.Temperature, Unit.Celsius, Unit.Fahrenheit);

        result.ConvertedValue.Should().Be(77m);
    }

    [Fact]
    public void Convert_WhenSourceIsFahrenheitAndTargetIsCelsius_ReturnsExpectedValue()
    {
        var result = _service.Convert(32m, ConversionType.Temperature, Unit.Fahrenheit, Unit.Celsius);

        result.ConvertedValue.Should().Be(0m);
    }

    [Fact]
    public void Convert_WhenSourceIsCelsiusAndTargetIsKelvin_ReturnsExpectedValue()
    {
        var result = _service.Convert(20m, ConversionType.Temperature, Unit.Celsius, Unit.Kelvin);

        result.ConvertedValue.Should().Be(293.15m);
    }

    [Fact]
    public void Convert_WhenSourceIsKelvinAndTargetIsCelsius_ReturnsExpectedValue()
    {
        var result = _service.Convert(273.15m, ConversionType.Temperature, Unit.Kelvin, Unit.Celsius);

        result.ConvertedValue.Should().Be(0m);
    }

    [Fact]
    public void Convert_WhenSourceIsKilometersPerHourAndTargetIsMetersPerSecond_ReturnsExpectedValue()
    {
        var result = _service.Convert(36m, ConversionType.Speed, Unit.KilometersPerHour, Unit.MetersPerSecond);

        result.ConvertedValue.Should().Be(10m);
    }

    [Fact]
    public void Convert_WhenSourceIsHoursAndTargetIsMinutes_ReturnsExpectedValue()
    {
        var result = _service.Convert(2m, ConversionType.Time, Unit.Hour, Unit.Minute);

        result.ConvertedValue.Should().Be(120m);
    }

    [Fact]
    public void Convert_WhenSourceIsDaysAndTargetIsSeconds_ReturnsExpectedValue()
    {
        var result = _service.Convert(1m, ConversionType.Time, Unit.Day, Unit.Second);

        result.ConvertedValue.Should().Be(86400m);
    }

    [Fact]
    public void Convert_WhenSourceAndTargetAreSameUnit_ReturnsOriginalValue()
    {
        var result = _service.Convert(13.5m, ConversionType.Length, Unit.Meter, Unit.Meter);

        result.ConvertedValue.Should().Be(13.5m);
    }

    [Fact]
    public void Convert_WhenConversionTypeIsUndefined_ThrowsUnsupportedConversionTypeException()
    {
        Action action = () => _service.Convert(5m, (ConversionType)999, Unit.Meter, Unit.Kilometer);

        action.Should().Throw<UnsupportedConversionTypeException>();
    }

    [Fact]
    public void Convert_WhenUnitIsUndefined_ThrowsInvalidUnitException()
    {
        Action action = () => _service.Convert(5m, ConversionType.Length, (Unit)999, Unit.Kilometer);

        action.Should().Throw<InvalidUnitException>();
    }

    [Fact]
    public void Convert_WhenLengthSourceIsUsedWithWeightTarget_ThrowsInvalidUnitException()
    {
        Action action = () => _service.Convert(5m, ConversionType.Length, Unit.Meter, Unit.Kilogram);

        action.Should().Throw<InvalidUnitException>();
    }

    [Fact]
    public void Convert_WhenTemperatureUnitIsUsedWithSpeedCategory_ThrowsInvalidUnitException()
    {
        Action action = () => _service.Convert(10m, ConversionType.Speed, Unit.Celsius, Unit.MetersPerSecond);

        action.Should().Throw<InvalidUnitException>();
    }

    [Fact]
    public void Convert_WhenRequestIsInvalid_ThrowsInvalidOperationException()
    {
        Action action = () => _service.Convert(decimal.MaxValue, ConversionType.Length, Unit.Meter, Unit.Kilometer);

        action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Convert_WhenSourceIsKilohertzAndTargetIsHertz_ReturnsExpectedValue()
    {
        var result = _service.Convert(2m, ConversionType.Frequency, Unit.Kilohertz, Unit.Hertz);

        result.ConvertedValue.Should().Be(2000m);
    }

    [Fact]
    public void Convert_WhenSourceIsHertzAndTargetIsKilohertz_ReturnsExpectedValue()
    {
        var result = _service.Convert(2000m, ConversionType.Frequency, Unit.Hertz, Unit.Kilohertz);

        result.ConvertedValue.Should().Be(2m);
    }

    [Fact]
    public void Convert_WhenSourceIsMegahertzAndTargetIsGigahertz_ReturnsExpectedValue()
    {
        var result = _service.Convert(2500m, ConversionType.Frequency, Unit.Megahertz, Unit.Gigahertz);

        // 2500 MHz = 2.5 GHz
        result.ConvertedValue.Should().Be(2.5m);
    }

    [Fact]
    public void Resolve_WhenStrategyIsMissing_ThrowsUnsupportedConversionTypeException()
    {
        var resolver = new ConversionStrategyResolver(new IUnitConversionStrategy[]
        {
            new LengthConversionStrategy(_unitRepository),
            new WeightConversionStrategy(_unitRepository)
        });

        Action action = () => resolver.Resolve(ConversionType.Time);

        action.Should().Throw<UnsupportedConversionTypeException>();
    }

    [Fact]
    public void Resolve_WhenDuplicateStrategiesAreRegistered_ThrowsInvalidOperationException()
    {
        Action action = () => new ConversionStrategyResolver(new IUnitConversionStrategy[]
        {
            new LengthConversionStrategy(_unitRepository),
            new LengthConversionStrategy(_unitRepository)
        });

        action.Should().Throw<InvalidOperationException>();
    }
}
