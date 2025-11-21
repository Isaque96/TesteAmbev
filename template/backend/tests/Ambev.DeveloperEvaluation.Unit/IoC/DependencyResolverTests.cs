using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.IoC;
using Ambev.DeveloperEvaluation.ORM;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.IoC;

public class DependencyResolverTests
{
    [Fact(DisplayName = "Given WebApplicationBuilder, when RegisterDependencies called, then initializes all modules")]
    public void Given_WebApplicationBuilder_When_RegisterDependencies_Then_InitializesAllModules()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder();
        builder.Services.AddDbContext<DefaultContext>(options =>
            options.UseInMemoryDatabase("TestDb"));

        // Act
        builder.RegisterDependencies();
        var app = builder.Build();

        // Assert
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        // Verifica ApplicationModule
        Assert.NotNull(services.GetService<IPasswordHasher>());

        // Verifica InfrastructureModule
        Assert.NotNull(services.GetService<IUserRepository>());
        Assert.NotNull(services.GetService<IProductRepository>());
        Assert.NotNull(services.GetService<ICategoryRepository>());
        Assert.NotNull(services.GetService<ICartRepository>());
        Assert.NotNull(services.GetService<DbContext>());

        // Verifica WebApiModule
        Assert.NotNull(services.GetService<HealthCheckService>());
    }

    [Fact(DisplayName = "Given WebApplicationBuilder, when RegisterDependencies called, then all services have correct lifetimes")]
    public void Given_WebApplicationBuilder_When_RegisterDependencies_Then_ServicesHaveCorrectLifetimes()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder();
        builder.Services.AddDbContext<DefaultContext>(options =>
            options.UseInMemoryDatabase("TestDb"));

        // Act
        builder.RegisterDependencies();

        // Assert
        // Singleton
        var passwordHasherDescriptor = builder.Services.FirstOrDefault(d => d.ServiceType == typeof(IPasswordHasher));
        Assert.NotNull(passwordHasherDescriptor);
        Assert.Equal(ServiceLifetime.Singleton, passwordHasherDescriptor.Lifetime);

        // Scoped
        var userRepoDescriptor = builder.Services.FirstOrDefault(d => d.ServiceType == typeof(IUserRepository));
        Assert.NotNull(userRepoDescriptor);
        Assert.Equal(ServiceLifetime.Scoped, userRepoDescriptor.Lifetime);

        var productRepoDescriptor = builder.Services.FirstOrDefault(d => d.ServiceType == typeof(IProductRepository));
        Assert.NotNull(productRepoDescriptor);
        Assert.Equal(ServiceLifetime.Scoped, productRepoDescriptor.Lifetime);

        var categoryRepoDescriptor = builder.Services.FirstOrDefault(d => d.ServiceType == typeof(ICategoryRepository));
        Assert.NotNull(categoryRepoDescriptor);
        Assert.Equal(ServiceLifetime.Scoped, categoryRepoDescriptor.Lifetime);

        var cartRepoDescriptor = builder.Services.FirstOrDefault(d => d.ServiceType == typeof(ICartRepository));
        Assert.NotNull(cartRepoDescriptor);
        Assert.Equal(ServiceLifetime.Scoped, cartRepoDescriptor.Lifetime);
    }

    [Fact(DisplayName = "Given WebApplicationBuilder, when RegisterDependencies called, then returns same builder instance")]
    public void Given_WebApplicationBuilder_When_RegisterDependencies_Then_ReturnsSameBuilderInstance()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder();
        builder.Services.AddDbContext<DefaultContext>(options =>
            options.UseInMemoryDatabase("TestDb"));

        // Act
        // RegisterDependencies é void, então só verificamos que não lança exceção
        var exception = Record.Exception(() => builder.RegisterDependencies());

        // Assert
        Assert.Null(exception);
    }

    [Fact(DisplayName = "Given WebApplicationBuilder, when RegisterDependencies called multiple times, then does not throw exception")]
    public void Given_WebApplicationBuilder_When_RegisterDependenciesCalledMultipleTimes_Then_DoesNotThrow()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder();
        builder.Services.AddDbContext<DefaultContext>(options =>
            options.UseInMemoryDatabase("TestDb"));

        // Act & Assert
        var exception = Record.Exception(() =>
        {
            builder.RegisterDependencies();
            builder.RegisterDependencies(); // Chama duas vezes
        });

        Assert.Null(exception);
    }
}
