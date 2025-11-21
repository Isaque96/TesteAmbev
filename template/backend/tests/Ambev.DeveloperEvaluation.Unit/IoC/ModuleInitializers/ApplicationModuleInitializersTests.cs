using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.IoC.ModuleInitializers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.IoC.ModuleInitializers;

public class ApplicationModuleInitializerTests
{
    [Fact(DisplayName = "Given WebApplicationBuilder, when Initialize called, then registers IPasswordHasher as singleton")]
    public void Given_WebApplicationBuilder_When_Initialize_Then_RegistersPasswordHasherAsSingleton()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder();
        var initializer = new ApplicationModuleInitializer();

        // Act
        initializer.Initialize(builder);
        var app = builder.Build();

        // Assert
        var passwordHasher = app.Services.GetService<IPasswordHasher>();
        Assert.NotNull(passwordHasher);
        Assert.IsType<BCryptPasswordHasher>(passwordHasher);
    }

    [Fact(DisplayName = "Given WebApplicationBuilder, when Initialize called, then IPasswordHasher is registered as singleton lifetime")]
    public void Given_WebApplicationBuilder_When_Initialize_Then_PasswordHasherIsSingleton()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder();
        var initializer = new ApplicationModuleInitializer();

        // Act
        initializer.Initialize(builder);

        // Assert
        var descriptor = builder.Services.FirstOrDefault(d => d.ServiceType == typeof(IPasswordHasher));
        Assert.NotNull(descriptor);
        Assert.Equal(ServiceLifetime.Singleton, descriptor!.Lifetime);
        Assert.Equal(typeof(BCryptPasswordHasher), descriptor.ImplementationType);
    }
}
