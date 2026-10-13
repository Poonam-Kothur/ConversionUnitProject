using Microsoft.Extensions.DependencyInjection;
using UnitConversion.Application.Factories;
using UnitConversion.Application.Interfaces;
using UnitConversion.Application.Services;
using UnitConversion.Application.Strategies;
using UnitConversion.Application.Validators;
using UnitConversion.Infrastructure.Repositories;

namespace UnitConversion.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddUnitConversionServices(this IServiceCollection services)
    {
        services.AddSingleton<IUnitRepository, UnitRepository>();
        services.AddSingleton<IUnitConversionStrategy, LengthConversionStrategy>();
        services.AddSingleton<IUnitConversionStrategy, WeightConversionStrategy>();
        services.AddSingleton<IUnitConversionStrategy, TemperatureConversionStrategy>();
        services.AddSingleton<IUnitConversionStrategy, SpeedConversionStrategy>();
        services.AddSingleton<IUnitConversionStrategy, TimeConversionStrategy>();
        services.AddSingleton<IUnitConversionStrategy, FrequencyConversionStrategy>();
        services.AddSingleton<ConversionStrategyResolver>();
        services.AddScoped<ConversionRequestValidator>();
        services.AddScoped<IConversionService, ConversionService>();
        return services;
    }
}
