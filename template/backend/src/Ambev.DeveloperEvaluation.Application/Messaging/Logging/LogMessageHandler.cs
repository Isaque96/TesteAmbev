using System.Text.Json;
using Ambev.DeveloperEvaluation.Domain.Messaging;
using Microsoft.Extensions.Logging;
using Rebus.Handlers;

namespace Ambev.DeveloperEvaluation.Application.Messaging.Logging;

public class LogMessageHandler(ILogger<LogMessageHandler> logger) : IHandleMessages<LogMessage>
{
    public Task Handle(LogMessage message)
    {
        logger.LogInformation(
            "Message: {Message} wihth content: {Content}",
            message.Message,
            message.MyObj == null ? "-" : JsonSerializer.Serialize(message.MyObj));
        
        return Task.CompletedTask;
    }
}