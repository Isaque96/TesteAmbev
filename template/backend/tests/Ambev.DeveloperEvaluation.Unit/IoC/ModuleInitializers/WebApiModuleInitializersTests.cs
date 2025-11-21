using Ambev.DeveloperEvaluation.IoC.ModuleInitializers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.IoC.ModuleInitializers;

public class WebApiModuleInitializerTests
{
    [Fact(DisplayName = "Given WebApplicationBuilder, when Initialize called, then registers controllers")]
    public void Given_WebApplicationBuilder_When_Initialize_Then_RegistersControllers()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder();
        var initializer = new WebApiModuleInitializer();

        // Act
        initializer.Initialize(builder);

        // Assert
        // Verifica se o serviço de controllers foi registrado
        var mvcBuilder = builder.Services.FirstOrDefault(d =>
            d.ServiceType.Name.Contains("MvcMarkerService") ||
            d.ServiceType.FullName?.Contains("Microsoft.AspNetCore.Mvc") == true);

        Assert.NotNull(mvcBuilder);
    }

    [Fact(DisplayName = "Given WebApplicationBuilder, when Initialize called, then registers health checks")]
    public void Given_WebApplicationBuilder_When_Initialize_Then_RegistersHealthChecks()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder();
        var initializer = new WebApiModuleInitializer();

        // Act
        initializer.Initialize(builder);
        var app = builder.Build();

        // Assert
        var healthCheckService = app.Services.GetService<HealthCheckService>();
        Assert.NotNull(healthCheckService);
    }

    [Fact(DisplayName = "Given WebApplicationBuilder, when Initialize called, then both controllers and health checks are registered")]
    public void Given_WebApplicationBuilder_When_Initialize_Then_BothServicesRegistered()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder();
        var initializer = new WebApiModuleInitializer();

        // Act
        initializer.Initialize(builder);
        var app = builder.Build();

        // Assert
        var healthCheckService = app.Services.GetService<HealthCheckService>();
        Assert.NotNull(healthCheckService);

        // Verifica se controllers foram registrados checando se há serviços MVC
        var hasMvcServices = builder.Services.Any(d =>
            d.ServiceType.FullName?.Contains("Microsoft.AspNetCore.Mvc") == true);
        Assert.True(hasMvcServices);
    }
}