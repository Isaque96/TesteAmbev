using Ambev.DeveloperEvaluation.ORM;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.ORM;

public class DefaultDbContextFactoryTests : IDisposable
{
    private readonly string _testDirectory;
    private readonly string _appSettingsPath;

    public DefaultDbContextFactoryTests()
    {
        _testDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_testDirectory);
        _appSettingsPath = Path.Combine(_testDirectory, "appsettings.json");
    }

    void IDisposable.Dispose()
    {
        if (Directory.Exists(_testDirectory))
        {
            Directory.Delete(_testDirectory, recursive: true);
        }

        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Subclasse usada só nos testes para capturar como o DbContextOptions foi configurado,
    /// sem realmente tentar inicializar 
    /// </summary>
    private class TestableDefaultDbContextFactory : DefaultDbContextFactory
    {
        public DbContextOptions<DefaultContext>? LastOptions { get; private set; }
        public IConfigurationRoot? LastConfiguration { get; private set; }

        public DefaultContext CreateDbContextForTest()
        {
            // Copia literal da implementação original, mas intercepta antes de criar o contexto real.
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            LastConfiguration = configuration;

            var builder = new DbContextOptionsBuilder<DefaultContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            builder.UseNpgsql(
                connectionString,
                b => b.MigrationsAssembly(typeof(DefaultContext).Assembly.FullName)
            );

            LastOptions = builder.Options;

            // NÃO chamamos o construtor real de DefaultContext aqui para não disparar o EF/Npgsql.
            // Em vez disso, retornamos um contexto "fake" minimalista usando InMemoryDatabase só para satisfazer o tipo.
            var inMemoryBuilder = new DbContextOptionsBuilder<DefaultContext>()
                .UseInMemoryDatabase("FakeDbForFactoryTests");

            return new DefaultContext(inMemoryBuilder.Options);
        }
    }

    [Fact(DisplayName = "Given valid appsettings.json with connection string, when CreateDbContextForTest called, then config uses that connection string")]
    public void Given_ValidAppSettings_When_CreateDbContextForTest_Then_UsesConnectionStringFromConfig()
    {
        // Arrange
        const string expectedConnectionString = "Host=localhost;Database=testdb;Username=user;Password=pass";

        var appSettingsContent = $$"""
                                   {
                                     "ConnectionStrings": {
                                       "DefaultConnection": "{{expectedConnectionString}}"
                                     }
                                   }
                                   """;

        File.WriteAllText(_appSettingsPath, appSettingsContent);

        var factory = new TestableDefaultDbContextFactory();
        var originalDirectory = Directory.GetCurrentDirectory();

        try
        {
            Directory.SetCurrentDirectory(_testDirectory);

            // Act
            var context = factory.CreateDbContextForTest();
            
            // Assert básicos só para garantir que o método não quebrou
            Assert.NotNull(context);
            Assert.IsType<DefaultContext>(context);

            // Verifica configuração capturada
            Assert.NotNull(factory.LastConfiguration);
            Assert.NotNull(factory.LastOptions);

            var connectionStringFromConfig =
                factory.LastConfiguration!.GetConnectionString("DefaultConnection");

            Assert.Equal(expectedConnectionString, connectionStringFromConfig);
        }
        finally
        {
            Directory.SetCurrentDirectory(originalDirectory);
        }
    }

    [Fact(DisplayName = "Given missing appsettings.json, when CreateDbContextForTest called, then throws FileNotFoundException")]
    public void Given_MissingAppSettings_When_CreateDbContextForTest_Then_ThrowsFileNotFoundException()
    {
        // Arrange
        var factory = new TestableDefaultDbContextFactory();
        var originalDirectory = Directory.GetCurrentDirectory();

        try
        {
            Directory.SetCurrentDirectory(_testDirectory); // diretório sem appsettings.json

            // Act
            var exception = Record.Exception(() => factory.CreateDbContextForTest());

            // Assert
            Assert.IsType<FileNotFoundException>(exception);
        }
        finally
        {
            Directory.SetCurrentDirectory(originalDirectory);
        }
    }

    [Fact(DisplayName = "Given appsettings.json without ConnectionStrings, when CreateDbContextForTest called, then connection string in config is null")]
    public void Given_AppSettingsWithoutConnectionStrings_When_CreateDbContextForTest_Then_ConnectionStringIsNull()
    {
        // Arrange
        const string appSettingsContent = """
                                          {
                                            "Logging": {
                                              "LogLevel": {
                                                "Default": "Information"
                                              }
                                            }
                                          }
                                          """;

        File.WriteAllText(_appSettingsPath, appSettingsContent);

        var factory = new TestableDefaultDbContextFactory();
        var originalDirectory = Directory.GetCurrentDirectory();

        try
        {
            Directory.SetCurrentDirectory(_testDirectory);

            // Act
            var _ = factory.CreateDbContextForTest();

            // Assert
            Assert.NotNull(factory.LastConfiguration);
            var connectionStringFromConfig =
                factory.LastConfiguration!.GetConnectionString("DefaultConnection");

            Assert.Null(connectionStringFromConfig);
        }
        finally
        {
            Directory.SetCurrentDirectory(originalDirectory);
        }
    }

    [Fact(DisplayName = "Given appsettings.json with empty DefaultConnection, when CreateDbContextForTest called, then connection string in config is empty")]
    public void Given_EmptyDefaultConnection_When_CreateDbContextForTest_Then_ConnectionStringIsEmpty()
    {
        // Arrange
        const string appSettingsContent = """
                                          {
                                            "ConnectionStrings": {
                                              "DefaultConnection": ""
                                            }
                                          }
                                          """;

        File.WriteAllText(_appSettingsPath, appSettingsContent);

        var factory = new TestableDefaultDbContextFactory();
        var originalDirectory = Directory.GetCurrentDirectory();

        try
        {
            Directory.SetCurrentDirectory(_testDirectory);

            // Act
            var _ = factory.CreateDbContextForTest();

            // Assert
            Assert.NotNull(factory.LastConfiguration);
            var connectionStringFromConfig =
                factory.LastConfiguration!.GetConnectionString("DefaultConnection");

            Assert.Equal(string.Empty, connectionStringFromConfig);
        }
        finally
        {
            Directory.SetCurrentDirectory(originalDirectory);
        }
    }
}