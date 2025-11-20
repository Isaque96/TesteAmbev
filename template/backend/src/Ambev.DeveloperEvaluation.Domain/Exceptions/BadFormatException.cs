namespace Ambev.DeveloperEvaluation.Domain.Exceptions;

public class BadFormatException : Exception
{
    public string? PropertyName { get; }
    public string? ExpectedFormat { get; }

    public BadFormatException() { }

    public BadFormatException(string message) : base(message) { }

    public BadFormatException(string propertyName, string expectedFormat)
        : base(JoinFormat(propertyName, expectedFormat))
    {
        PropertyName = propertyName;
        ExpectedFormat = expectedFormat;
    }

    public BadFormatException(string message, Exception inner) : base(message, inner) { }

    public BadFormatException(string message, string expectedFormat, Exception inner)
        : base(JoinFormat(message, expectedFormat), inner)
    {
        ExpectedFormat = expectedFormat;
    }

    private static string JoinFormat(string propertyName, string expectedFormat)
    {
        return $"The property '{propertyName}' has a bad format.{Environment.NewLine}Expected format: {expectedFormat}";
    }
}