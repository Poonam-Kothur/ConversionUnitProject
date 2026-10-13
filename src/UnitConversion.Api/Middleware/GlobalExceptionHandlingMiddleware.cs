using System.Net;
using Microsoft.AspNetCore.Mvc;
using UnitConversion.Domain.Exceptions;

namespace UnitConversion.Api.Middleware;

public sealed class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            var problem = CreateProblemDetails(context, exception);
            context.Response.StatusCode = problem.Status ?? (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/problem+json";
            await context.Response.WriteAsJsonAsync(problem);
        }
    }

    private static ProblemDetails CreateProblemDetails(HttpContext context, Exception exception)
    {
        var statusCode = exception switch
        {
            UnsupportedConversionTypeException => StatusCodes.Status400BadRequest,
            InvalidUnitException => StatusCodes.Status400BadRequest,
            InvalidOperationException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        var detail = statusCode == StatusCodes.Status500InternalServerError
            ? "An unexpected server error occurred."
            : exception.Message;

        return new ProblemDetails
        {
            Type = "https://httpstatuses.com/" + statusCode,
            Title = statusCode == StatusCodes.Status500InternalServerError ? "Server Error" : "Bad Request",
            Status = statusCode,
            Detail = detail,
            Instance = context.Request.Path
        };
    }
}
