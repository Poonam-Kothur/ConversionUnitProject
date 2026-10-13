using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using UnitConversion.Api.Contracts.Requests;
using UnitConversion.Api.Contracts.Responses;

namespace UnitConversion.IntegrationTests;

public class ConversionApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public ConversionApiTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task PostConvert_WhenRequestIsValid_ReturnsOkAndExpectedPayload()
    {
        var client = _factory.CreateClient();
        var request = new ConversionRequest
        {
            Value = 100m,
            ConversionType = "Length",
            FromUnit = "Meter",
            ToUnit = "Kilometer"
        };

        var response = await client.PostAsJsonAsync("/api/conversions", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var payload = await response.Content.ReadFromJsonAsync<ConversionResponse>();
        payload.Should().NotBeNull();
        payload!.ConvertedValue.Should().Be(0.1m);
    }

    [Fact]
    public async Task PostConvert_WhenTemperatureRequestIsValid_ReturnsOk()
    {
        var client = _factory.CreateClient();
        var request = new ConversionRequest
        {
            Value = 100m,
            ConversionType = "Temperature",
            FromUnit = "Celsius",
            ToUnit = "Fahrenheit"
        };

        var response = await client.PostAsJsonAsync("/api/conversions", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var payload = await response.Content.ReadFromJsonAsync<ConversionResponse>();
        payload!.ConvertedValue.Should().Be(212m);
    }

    [Fact]
    public async Task PostConvert_WhenSpeedRequestIsValid_ReturnsOk()
    {
        var client = _factory.CreateClient();
        var request = new ConversionRequest
        {
            Value = 36m,
            ConversionType = "Speed",
            FromUnit = "KilometersPerHour",
            ToUnit = "MetersPerSecond"
        };

        var response = await client.PostAsJsonAsync("/api/conversions", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var payload = await response.Content.ReadFromJsonAsync<ConversionResponse>();
        payload!.ConvertedValue.Should().Be(10m);
    }

    [Fact]
    public async Task PostConvert_WhenTimeRequestIsValid_ReturnsOk()
    {
        var client = _factory.CreateClient();
        var request = new ConversionRequest
        {
            Value = 2m,
            ConversionType = "Time",
            FromUnit = "Hour",
            ToUnit = "Minute"
        };

        var response = await client.PostAsJsonAsync("/api/conversions", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var payload = await response.Content.ReadFromJsonAsync<ConversionResponse>();
        payload!.ConvertedValue.Should().Be(120m);
    }

    [Fact]
    public async Task PostConvert_WhenSourceAndTargetAreSameUnit_ReturnsIdentityValue()
    {
        var client = _factory.CreateClient();
        var request = new ConversionRequest
        {
            Value = 21m,
            ConversionType = "Length",
            FromUnit = "Meter",
            ToUnit = "Meter"
        };

        var response = await client.PostAsJsonAsync("/api/conversions", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var payload = await response.Content.ReadFromJsonAsync<ConversionResponse>();
        payload!.ConvertedValue.Should().Be(21m);
    }

    [Fact]
    public async Task PostConvert_WhenJsonIsInvalid_ReturnsBadRequest()
    {
        var client = _factory.CreateClient();
        var request = new StringContent("{ invalid json }", System.Text.Encoding.UTF8, "application/json");

        var response = await client.PostAsync("/api/conversions", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PostConvert_WhenRequiredInputsAreMissing_ReturnsBadRequest()
    {
        var client = _factory.CreateClient();
        var body = new { value = 5m };

        var response = await client.PostAsJsonAsync("/api/conversions", body);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PostConvert_WhenUnknownConversionType_ReturnsBadRequest()
    {
        var client = _factory.CreateClient();
        var request = new ConversionRequest
        {
            Value = 1m,
            ConversionType = "Volume",
            FromUnit = "Meter",
            ToUnit = "Kilometer"
        };

        var response = await client.PostAsJsonAsync("/api/conversions", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PostConvert_WhenUnknownUnit_ReturnsBadRequest()
    {
        var client = _factory.CreateClient();
        var request = new ConversionRequest
        {
            Value = 10m,
            ConversionType = "Length",
            FromUnit = "UnknownUnit",
            ToUnit = "Meter"
        };

        var response = await client.PostAsJsonAsync("/api/conversions", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PostConvert_WhenUnitsAreIncompatible_ReturnsBadRequest()
    {
        var client = _factory.CreateClient();
        var request = new ConversionRequest
        {
            Value = 10m,
            ConversionType = "Length",
            FromUnit = "Meter",
            ToUnit = "Pound"
        };

        var response = await client.PostAsJsonAsync("/api/conversions", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetOptions_ReturnsAllCategoriesAndUnits()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/conversions/options");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var payload = await response.Content.ReadFromJsonAsync<ConversionOptionsResponse>();
        payload.Should().NotBeNull();
        payload!.Options.Should().NotBeEmpty();
        payload.Options.Any(option => option.ConversionType == "Length").Should().BeTrue();
    }
}
