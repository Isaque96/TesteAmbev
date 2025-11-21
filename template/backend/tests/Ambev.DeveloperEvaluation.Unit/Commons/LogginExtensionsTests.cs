using Ambev.DeveloperEvaluation.Common.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Commons;

public class LoggingExtensionTests
{
    #region FilterPredicate

    [Fact(DisplayName = "Given information log with 200 and /health, when filter applied, then it is excluded")]
    public void Given_Info200Health_When_FilterPredicate_Then_Excluded()
    {
        // Arrange
        var properties = new Dictionary<string, LogEventPropertyValue>
        {
            { "StatusCode", new ScalarValue("200") },
            { "Path",       new ScalarValue("/health") }
        };

        var logEvent = new LogEvent(
            DateTimeOffset.Now,
            LogEventLevel.Information,
            exception: null,
            messageTemplate: new MessageTemplate("", []),
            properties: properties.Select(p => new LogEventProperty(p.Key, p.Value))
        );

        // Act
        var result = InvokeFilterPredicate(logEvent);

        // Assert
        Assert.False(result); // excluído
    }

    [Fact(DisplayName = "Given information log with non-200, when filter applied, then it is not excluded")]
    public void Given_InfoNon200_When_FilterPredicate_Then_NotExcluded()
    {
        // Arrange
        var properties = new Dictionary<string, LogEventPropertyValue>
        {
            { "StatusCode", new ScalarValue("500") },
            { "Path",       new ScalarValue("/health") }
        };

        var logEvent = new LogEvent(
            DateTimeOffset.Now,
            LogEventLevel.Information,
            exception: null,
            messageTemplate: new MessageTemplate("", []),
            properties: properties.Select(x => new LogEventProperty(x.Key, x.Value))
        );

        // Act
        var result = InvokeFilterPredicate(logEvent);

        // Assert
        Assert.False(result); // não excluído
    }
    
    /// <summary>
    /// Hack simples para invocar o FilterPredicate private via reflection.
    /// </summary>
    private static bool InvokeFilterPredicate(LogEvent logEvent)
    {
        var loggingExtensionType = typeof(LoggingExtension);
        var field = loggingExtensionType.GetField(
            "FilterPredicate",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

        Assert.NotNull(field);

        var predicate = (Func<LogEvent, bool>)field.GetValue(null)!;
        return predicate(logEvent);
    }

    #endregion

    #region AddDefaultLogging

    [Fact(DisplayName = "Given WebApplicationBuilder, when AddDefaultLogging called, then logging services are registered")]
    public void Given_Builder_When_AddDefaultLogging_Then_RegistersLogging()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            EnvironmentName = Environments.Development,
            ApplicationName = "TestApp"
        });

        // Act
        builder.AddDefaultLogging();
        var app = builder.Build();

        // Assert
        using var scope = app.Services.CreateScope();
        var loggerFactory = scope.ServiceProvider.GetService<ILoggerFactory>();

        Assert.NotNull(loggerFactory);
    }

    #endregion

    #region UseDefaultLogging

    [Fact(DisplayName = "Given app with logging, when UseDefaultLogging called, then info is logged")]
    public void Given_App_When_UseDefaultLogging_Then_LogsInformation()
    {
        // Arrange
        var logEvents = new List<LogEvent>();
        var sink = new CollectingSink(logEvents);

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Sink(sink)
            .CreateLogger();

        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            EnvironmentName = Environments.Development,
            ApplicationName = "TestApp"
        });

        var app = builder.Build();

        // Act
        app.UseDefaultLogging();

        // Assert
        Assert.Contains(logEvents, e =>
            e.MessageTemplate.Text.Contains("Logging enabled for")
        );
    }

    private class CollectingSink(IList<LogEvent> events) : ILogEventSink
    {
        public void Emit(LogEvent logEvent) => events.Add(logEvent);
    }

    #endregion
}