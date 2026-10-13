using Microsoft.AspNetCore.Mvc;
using UnitConversion.Api.Contracts.Requests;
using UnitConversion.Api.Contracts.Responses;
using UnitConversion.Application.Interfaces;
using UnitConversion.Domain.Enums;
using UnitConversion.Domain.Exceptions;

namespace UnitConversion.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ConversionsController : ControllerBase
{
    private readonly IConversionService _conversionService;

    public ConversionsController(IConversionService conversionService)
    {
        _conversionService = conversionService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ConversionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public ActionResult<ConversionResponse> Convert([FromBody] ConversionRequest request)
    {
        if (request is null)
        {
            throw new InvalidOperationException("Request body is required.");
        }

        if (request.Value is null)
        {
            throw new InvalidOperationException("The 'value' field is required.");
        }

        if (string.IsNullOrWhiteSpace(request.ConversionType))
        {
            throw new UnsupportedConversionTypeException("The 'conversionType' field is required.");
        }

        if (string.IsNullOrWhiteSpace(request.FromUnit))
        {
            throw new InvalidUnitException("The 'fromUnit' field is required.");
        }

        if (string.IsNullOrWhiteSpace(request.ToUnit))
        {
            throw new InvalidUnitException("The 'toUnit' field is required.");
        }

        if (!Enum.TryParse<ConversionType>(request.ConversionType, true, out var conversionType))
        {
            throw new UnsupportedConversionTypeException($"Unsupported conversion type '{request.ConversionType}'.");
        }

        if (!Enum.TryParse<Unit>(request.FromUnit, true, out var fromUnit))
        {
            throw new InvalidUnitException($"Unknown source unit '{request.FromUnit}'.");
        }

        if (!Enum.TryParse<Unit>(request.ToUnit, true, out var toUnit))
        {
            throw new InvalidUnitException($"Unknown target unit '{request.ToUnit}'.");
        }

        var result = _conversionService.Convert(request.Value.Value, conversionType, fromUnit, toUnit);

        return Ok(new ConversionResponse
        {
            OriginalValue = result.OriginalValue,
            ConversionType = result.ConversionType.ToString(),
            FromUnit = result.FromUnit.ToString(),
            ToUnit = result.ToUnit.ToString(),
            ConvertedValue = result.ConvertedValue
        });
    }

    [HttpGet("options")]
    [ProducesResponseType(typeof(ConversionOptionsResponse), StatusCodes.Status200OK)]
    public ActionResult<ConversionOptionsResponse> GetOptions()
    {
        var options = _conversionService.GetAvailableOptions();
        var response = new ConversionOptionsResponse
        {
            Options = options
                .OrderBy(pair => pair.Key)
                .Select(pair => new ConversionOption
                {
                    ConversionType = pair.Key.ToString(),
                    Units = pair.Value.Select(unit => unit.ToString()).ToArray()
                })
                .ToArray()
        };

        return Ok(response);
    }
}
