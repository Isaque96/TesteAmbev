using System.Net;
using System.Text.Json;
using Ambev.DeveloperEvaluation.Common.HealthChecks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Commons;

public class HealthCheckExtensionsTests
{
    #region AddBasicHealthChecks Tests

    [Fact(DisplayName = "Given WebApplicationBuilder, when AddBasicHealthChecks called, then registers Liveness and Readiness health checks")]
    public void Given_WebApplicationBuilder_When_AddBasicHealthChecks_Then_RegistersLivenessAndReadinessHealthChecks()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder();

        // Act
        builder.AddBasicHealthChecks();

        // Assert
        var app = builder.Build();
        var healthCheckService = app.Services.GetRequiredService<HealthCheckService>();
        Assert.NotNull(healthCheckService);

        // Valida que os health checks foram registrados
        var registrations = app.Services.GetRequiredService<IOptions<HealthCheckServiceOptions>>().Value.Registrations;

        var livenessCheck = registrations.FirstOrDefault(r => r.Name == "Liveness");
        Assert.NotNull(livenessCheck);
        Assert.Contains("liveness", livenessCheck.Tags);

        var readinessCheck = registrations.FirstOrDefault(r => r.Name == "Readiness");
        Assert.NotNull(readinessCheck);
        Assert.Contains("readiness", readinessCheck.Tags);
    }

    [Fact(DisplayName = "Given WebApplicationBuilder, when AddBasicHealthChecks called, then Liveness check returns Healthy")]
    public async Task Given_WebApplicationBuilder_When_AddBasicHealthChecks_Then_LivenessCheckReturnsHealthy()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder();
        builder.AddBasicHealthChecks();
        var app = builder.Build();

        // Act
        var healthCheckService = app.Services.GetRequiredService<HealthCheckService>();
        var result = await healthCheckService.CheckHealthAsync(check => check.Tags.Contains("liveness"));

        // Assert
        Assert.Equal(HealthStatus.Healthy, result.Status);
        Assert.Contains("Liveness", result.Entries.Keys);
        Assert.Equal(HealthStatus.Healthy, result.Entries["Liveness"].Status);
    }

    [Fact(DisplayName = "Given WebApplicationBuilder, when AddBasicHealthChecks called, then Readiness check returns Healthy")]
    public async Task Given_WebApplicationBuilder_When_AddBasicHealthChecks_Then_ReadinessCheckReturnsHealthy()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder();
        builder.AddBasicHealthChecks();
        var app = builder.Build();

        // Act
        var healthCheckService = app.Services.GetRequiredService<HealthCheckService>();
        var result = await healthCheckService.CheckHealthAsync(check => check.Tags.Contains("readiness"));

        // Assert
        Assert.Equal(HealthStatus.Healthy, result.Status);
        Assert.Contains("Readiness", result.Entries.Keys);
        Assert.Equal(HealthStatus.Healthy, result.Entries["Readiness"].Status);
    }

    #endregion

    #region UseBasicHealthChecks Tests

    [Fact(DisplayName = "Given WebApplication with health checks, when UseBasicHealthChecks called, then /health/live endpoint returns 200 OK")]
    public async Task Given_WebApplicationWithHealthChecks_When_UseBasicHealthChecks_Then_LivenessEndpointReturns200()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder();
        builder.AddBasicHealthChecks();
        builder.WebHost.UseTestServer();

        var app = builder.Build();
        app.UseBasicHealthChecks();
        await app.StartAsync();

        var client = app.GetTestClient();

        // Act
        var response = await client.GetAsync("/health/live");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        var json = JsonDocument.Parse(content);

        Assert.Equal("Healthy", json.RootElement.GetProperty("status").GetString());
        Assert.True(json.RootElement.GetProperty("healthChecks").GetArrayLength() > 0);
    }

    [Fact(DisplayName = "Given WebApplication with health checks, when UseBasicHealthChecks called, then /health/ready endpoint returns 200 OK")]
    public async Task Given_WebApplicationWithHealthChecks_When_UseBasicHealthChecks_Then_ReadinessEndpointReturns200()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder();
        builder.AddBasicHealthChecks();
        builder.WebHost.UseTestServer();

        var app = builder.Build();
        app.UseBasicHealthChecks();
        await app.StartAsync();

        var client = app.GetTestClient();

        // Act
        var response = await client.GetAsync("/health/ready");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        var json = JsonDocument.Parse(content);

        Assert.Equal("Healthy", json.RootElement.GetProperty("status").GetString());
        Assert.True(json.RootElement.GetProperty("healthChecks").GetArrayLength() > 0);
    }

    [Fact(DisplayName = "Given WebApplication with health checks, when UseBasicHealthChecks called, then /health endpoint returns 200 OK")]
    public async Task Given_WebApplicationWithHealthChecks_When_UseBasicHealthChecks_Then_HealthEndpointReturns200()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder();
        builder.AddBasicHealthChecks();
        builder.WebHost.UseTestServer();

        var app = builder.Build();
        app.UseBasicHealthChecks();
        await app.StartAsync();

        var client = app.GetTestClient();

        // Act
        var response = await client.GetAsync("/health");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        var json = JsonDocument.Parse(content);

        Assert.Equal("Healthy", json.RootElement.GetProperty("status").GetString());
    }

    [Fact(DisplayName = "Given WebApplication with health checks, when UseBasicHealthChecks called, then response contains environment name")]
    public async Task Given_WebApplicationWithHealthChecks_When_UseBasicHealthChecks_Then_ResponseContainsEnvironmentName()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder();
        builder.AddBasicHealthChecks();
        builder.WebHost.UseTestServer();

        var app = builder.Build();
        app.UseBasicHealthChecks();
        await app.StartAsync();

        var client = app.GetTestClient();

        // Act
        var response = await client.GetAsync("/health/live");

        // Assert
        var content = await response.Content.ReadAsStringAsync();
        var json = JsonDocument.Parse(content);

        var healthChecks = json.RootElement.GetProperty("healthChecks").EnumerateArray().ToList();
        Assert.NotEmpty(healthChecks);

        var firstCheck = healthChecks[0];
        Assert.True(firstCheck.TryGetProperty("hostEnvironment", out var envProperty));
        Assert.False(string.IsNullOrWhiteSpace(envProperty.GetString()));
    }

    [Fact(DisplayName = "Given WebApplication with health checks, when UseBasicHealthChecks called, then response content type is application/json")]
    public async Task Given_WebApplicationWithHealthChecks_When_UseBasicHealthChecks_Then_ResponseContentTypeIsJson()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder();
        builder.AddBasicHealthChecks();
        builder.WebHost.UseTestServer();

        var app = builder.Build();
        app.UseBasicHealthChecks();
        await app.StartAsync();

        var client = app.GetTestClient();

        // Act
        var response = await client.GetAsync("/health");

        // Assert
        Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
    }

    #endregion
}