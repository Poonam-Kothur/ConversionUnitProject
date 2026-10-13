using Microsoft.OpenApi.Models;
using UnitConversion.Api.Middleware;
using UnitConversion.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "UnitConversion API",
        Version = "v1"
    });
});
builder.Services.AddUnitConversionServices();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "UnitConversion API v1");
    });
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

public partial class Program;

