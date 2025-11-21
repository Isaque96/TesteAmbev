using System.Drawing;

namespace Ambev.DeveloperEvaluation.Domain.Messaging;

public class LogMessage
{
    public LogMessage() { }

    public LogMessage(string message, object? myObj)
    {
        Message = message;
        MyObj = myObj;
    }

    public string Message { get; set; } = string.Empty;
    public object? MyObj { get; set; }
}