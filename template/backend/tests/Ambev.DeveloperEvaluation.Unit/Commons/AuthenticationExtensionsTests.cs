using System.Text;
using Ambev.DeveloperEvaluation.Common.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Commons;

public class AuthenticationExtensionTests
{
    #region Helper

    private static IConfiguration BuildConfiguration(string? secretKey)
    {
        var inMemory = new[]
        {
            new KeyValuePair<string, string?>("Jwt:SecretKey", secretKey)
        };

        return new ConfigurationBuilder()
            .AddInMemoryCollection(inMemory)
            .Build();
    }

    #endregion

    [Fact(DisplayName = "Given null or empty secret key, when AddJwtAuthentication called, then throws ArgumentException")]
    public void Given_NullOrEmptySecretKey_When_AddJwtAuthentication_Then_ThrowsArgumentException()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = BuildConfiguration(secretKey: null);

        // Act + Assert
        var ex = Assert.Throws<ArgumentNullException>(() =>
            services.AddJwtAuthentication(configuration));

        // Opcional: se quiser, pode garantir que a mensagem não está vazia
        Assert.False(string.IsNullOrWhiteSpace(ex.Message));
    }

    [Fact(DisplayName = "Given valid secret key, when AddJwtAuthentication called, then returns same service collection instance")]
    public void Given_ValidSecretKey_When_AddJwtAuthentication_Then_ReturnsSameServiceCollection()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = BuildConfiguration("my-secret-key");

        // Act
        var result = services.AddJwtAuthentication(configuration);

        // Assert
        Assert.Same(services, result);
    }

    [Fact(DisplayName = "Given valid secret key, when AddJwtAuthentication called, then configures JwtBearer authentication")]
    public void Given_ValidSecretKey_When_AddJwtAuthentication_Then_RegistersJwtBearerAuthentication()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = BuildConfiguration("my-secret-key");

        // registra IConfiguration no DI, para o JwtTokenGenerator conseguir receber via construtor
        services.AddSingleton(configuration);

        // Act
        services.AddJwtAuthentication(configuration);
        var provider = services.BuildServiceProvider();

        // Assert
        // 1. Verifica se IJwtTokenGenerator foi registrado
        var jwtTokenGenerator = provider.GetService<IJwtTokenGenerator>();
        Assert.NotNull(jwtTokenGenerator);
        Assert.IsType<JwtTokenGenerator>(jwtTokenGenerator);

        // 2. Verifica se o JwtBearerOptions foi configurado com TokenValidationParameters esperados
        var jwtOptionsMonitor = provider.GetService<
            Microsoft.Extensions.Options.IOptionsMonitor<JwtBearerOptions>>();

        Assert.NotNull(jwtOptionsMonitor);

        var jwtOptions = jwtOptionsMonitor.Get(JwtBearerDefaults.AuthenticationScheme);

        Assert.False(jwtOptions.RequireHttpsMetadata);
        Assert.True(jwtOptions.SaveToken);

        Assert.NotNull(jwtOptions.TokenValidationParameters);
        var tvp = jwtOptions.TokenValidationParameters;

        Assert.True(tvp.ValidateIssuerSigningKey);
        Assert.IsType<SymmetricSecurityKey>(tvp.IssuerSigningKey);
        Assert.False(tvp.ValidateIssuer);
        Assert.False(tvp.ValidateAudience);
        Assert.Equal(TimeSpan.Zero, tvp.ClockSkew);

        var symmetricKey = (SymmetricSecurityKey)tvp.IssuerSigningKey!;
        var expectedKeyBytes = Encoding.ASCII.GetBytes("my-secret-key");
        Assert.Equal(expectedKeyBytes, symmetricKey.Key);
    }
}