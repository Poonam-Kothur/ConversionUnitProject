# Unit Conversion API

This project implements a production-minded ASP.NET Core REST API for converting values between units across several categories including length, weight, temperature, speed, and time.

## Overview

The API follows a layered architecture and uses the Strategy design pattern to resolve the correct conversion algorithm at runtime based on the selected conversion type.

## Technology Stack

- ASP.NET Core 8
- C#
- xUnit

## Solution Structure

```text
UnitConversion.sln
src/
  UnitConversion.Api/
    Controllers/
    Contracts/
    Middleware/
    Program.cs
    appsettings.json
  UnitConversion.Application/
    Interfaces/
    Services/
    Strategies/
    Factories/
    Validators/
  UnitConversion.Domain/
    Enums/
    Models/
    Exceptions/
  UnitConversion.Infrastructure/
    Repositories/
    Catalog/
    DependencyInjection.cs
tests/
  UnitConversion.UnitTests/
  UnitConversion.IntegrationTests/
README.md
```

## Architecture and Dependency Flow

- Controllers depend only on the application service interface.
- The application layer depends on abstractions and the strategy resolver.
- The infrastructure layer implements the repository and provides DI registration.
- Strategy implementations are discovered through dependency injection and resolved using a dictionary-backed resolver.

## Strategy Pattern

Each conversion category has a dedicated strategy class:

- LengthConversionStrategy
- WeightConversionStrategy
- TemperatureConversionStrategy
- SpeedConversionStrategy
- TimeConversionStrategy

These are registered with dependency injection and resolved by `ConversionStrategyResolver`, which builds a dictionary keyed by `ConversionType` and throws a meaningful exception when a strategy is missing or duplicated.

## Unit Enum and Unit Catalog

A single `Unit` enum contains every supported unit. The actual metadata is centralized in the `UnitCatalog` and `UnitRepository` abstraction so there is one authoritative source of truth for category membership and conversion factors.

This avoids scattering conversion factors across controllers or strategy classes, and supports adding new units without rewriting the API contracts.

## Category-Dependent Unit Validation

The repository and validator ensure that:

- Units belong to the selected conversion category.
- Unsupported categories are rejected.
- Units from different categories cannot be mixed.
- Temperature operations use offset-aware formulas.

## Conversion Formulas

Base units used:

- Length: Meter
- Weight: Kilogram
- Speed: Meters per second
- Time: Second
- Temperature: Celsius as the internal reference scale

Examples:

- Length: `converted = value * sourceFactor / targetFactor`
- Weight: `converted = value * sourceFactor / targetFactor`
- Speed: `converted = value * sourceFactor / targetFactor`
- Time: `converted = value * sourceFactor / targetFactor`
- Temperature: `C = (F - 32) * 5/9`, `F = C * 9/5 + 32`, `K = C + 273.15`

## API Endpoints

### POST /api/conversions

Request example:

```json
{
  "value": 100,
  "conversionType": "Length",
  "fromUnit": "Meter",
  "toUnit": "Kilometer"
}
```

Response example:

```json
{
  "originalValue": 100,
  "conversionType": "Length",
  "fromUnit": "Meter",
  "toUnit": "Kilometer",
  "convertedValue": 0.1
}
```

### GET /api/conversions/options

Returns each conversion category and its valid units.

## Swagger

When running locally in development, Swagger is available at:

- `https://localhost:5001/swagger`
- or `http://localhost:5000/swagger` depending on launch settings

## Prerequisites

- .NET 10 SDK
- A local terminal or IDE such as Visual Studio or VS Code

## Local Setup and Run

```bash
dotnet restore UnitConversion.sln
dotnet build UnitConversion.sln
dotnet test UnitConversion.sln
dotnet run --project src/UnitConversion.Api
```

## Tests

Run the automated test suite with:

```bash
dotnet test UnitConversion.sln
```

## Error Handling and Validation

The API validates missing or invalid fields, unknown categories, undefined units, incompatible source and target unit categories, and invalid numeric requests. All validation failures are surfaced with standardized `ProblemDetails` responses and HTTP 400 status codes.

## Design Decisions and Trade-offs

- Decimal is used for conversion calculations to reduce rounding issues compared with floating-point types.
- Temperature conversions are implemented with offset-aware formulas rather than linear-only conversions.
- The central unit catalog allows the project to scale to hundreds of units without duplicating formulas.

## How to Add a New Unit

1. Add the enum member to the `Unit` enum.
2. Add the `UnitDefinition` entry in `UnitCatalog`.
3. Ensure the unit belongs to the correct `ConversionType`.
4. If needed, add or update the relevant strategy logic.

## How to Add a New Conversion Category

1. Add a new item to `ConversionType`.
2. Add units to the `Unit` enum.
3. Add metadata entries in `UnitCatalog`.
4. Create a new `IUnitConversionStrategy` implementation.
5. Register the strategy in `DependencyInjection`.

## Future Enhancements

- Store unit definitions in a database or configuration file.
- Add per-category validation rules and extensible metadata.
- Expand the catalog to support additional measurement systems and aliases.
- Add authentication and authorization if this API ever becomes externally exposed.
