using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.IoC.ModuleInitializers;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.IoC.ModuleInitializers;

public class InfrastructureModuleInitializerTests
{
    [Fact(DisplayName = "Given WebApplicationBuilder, when Initialize called, then registers DbContext as scoped")]
    public void Given_WebApplicationBuilder_When_Initialize_Then_RegistersDbContextAsScoped()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder();

        // Registra DefaultContext para o teste funcionar
        builder.Services.AddDbContext<DefaultContext>(options =>
            options.UseInMemoryDatabase("TestDb"));

        var initializer = new InfrastructureModuleInitializer();

        // Act
        initializer.Initialize(builder);

        // Assert
        var descriptor = builder.Services.FirstOrDefault(d => d.ServiceType == typeof(DbContext));
        Assert.NotNull(descriptor);
        Assert.Equal(ServiceLifetime.Scoped, descriptor.Lifetime);
    }

    [Fact(DisplayName = "Given WebApplicationBuilder, when Initialize called, then registers IUserRepository as scoped")]
    public void Given_WebApplicationBuilder_When_Initialize_Then_RegistersUserRepositoryAsScoped()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder();
        builder.Services.AddDbContext<DefaultContext>(options =>
            options.UseInMemoryDatabase("TestDb"));

        var initializer = new InfrastructureModuleInitializer();

        // Act
        initializer.Initialize(builder);

        // Assert
        var descriptor = builder.Services.FirstOrDefault(d => d.ServiceType == typeof(IUserRepository));
        Assert.NotNull(descriptor);
        Assert.Equal(ServiceLifetime.Scoped, descriptor.Lifetime);
        Assert.Equal(typeof(UserRepository), descriptor.ImplementationType);
    }

    [Fact(DisplayName = "Given WebApplicationBuilder, when Initialize called, then registers IProductRepository as scoped")]
    public void Given_WebApplicationBuilder_When_Initialize_Then_RegistersProductRepositoryAsScoped()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder();
        builder.Services.AddDbContext<DefaultContext>(options =>
            options.UseInMemoryDatabase("TestDb"));

        var initializer = new InfrastructureModuleInitializer();

        // Act
        initializer.Initialize(builder);

        // Assert
        var descriptor = builder.Services.FirstOrDefault(d => d.ServiceType == typeof(IProductRepository));
        Assert.NotNull(descriptor);
        Assert.Equal(ServiceLifetime.Scoped, descriptor.Lifetime);
        Assert.Equal(typeof(ProductRepository), descriptor.ImplementationType);
    }

    [Fact(DisplayName = "Given WebApplicationBuilder, when Initialize called, then registers ICategoryRepository as scoped")]
    public void Given_WebApplicationBuilder_When_Initialize_Then_RegistersCategoryRepositoryAsScoped()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder();
        builder.Services.AddDbContext<DefaultContext>(options =>
            options.UseInMemoryDatabase("TestDb"));

        var initializer = new InfrastructureModuleInitializer();

        // Act
        initializer.Initialize(builder);

        // Assert
        var descriptor = builder.Services.FirstOrDefault(d => d.ServiceType == typeof(ICategoryRepository));
        Assert.NotNull(descriptor);
        Assert.Equal(ServiceLifetime.Scoped, descriptor.Lifetime);
        Assert.Equal(typeof(CategoryRepository), descriptor.ImplementationType);
    }

    [Fact(DisplayName = "Given WebApplicationBuilder, when Initialize called, then registers ICartRepository as scoped")]
    public void Given_WebApplicationBuilder_When_Initialize_Then_RegistersCartRepositoryAsScoped()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder();
        builder.Services.AddDbContext<DefaultContext>(options =>
            options.UseInMemoryDatabase("TestDb"));

        var initializer = new InfrastructureModuleInitializer();

        // Act
        initializer.Initialize(builder);

        // Assert
        var descriptor = builder.Services.FirstOrDefault(d => d.ServiceType == typeof(ICartRepository));
        Assert.NotNull(descriptor);
        Assert.Equal(ServiceLifetime.Scoped, descriptor.Lifetime);
        Assert.Equal(typeof(CartRepository), descriptor.ImplementationType);
    }

    [Fact(DisplayName = "Given WebApplicationBuilder, when Initialize called, then all repositories are registered")]
    public void Given_WebApplicationBuilder_When_Initialize_Then_AllRepositoriesRegistered()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            EnvironmentName = Environments.Development,
            ApplicationName = "TestApp"
        });

        builder.Services.AddDbContext<DefaultContext>(options =>
            options.UseInMemoryDatabase("TestDb"));

        var initializer = new InfrastructureModuleInitializer();

        // Act
        initializer.Initialize(builder);
        var app = builder.Build();

        // Assert
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        Assert.NotNull(services.GetService<IUserRepository>());
        Assert.NotNull(services.GetService<IProductRepository>());
        Assert.NotNull(services.GetService<ICategoryRepository>());
        Assert.NotNull(services.GetService<ICartRepository>());
        Assert.NotNull(services.GetService<DbContext>());
    }
}
