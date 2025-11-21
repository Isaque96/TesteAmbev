using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Exceptions;

public class BadFormatExceptionTests
{
    [Fact(DisplayName = "Given parameterless constructor, when exception created, then Message is default and properties are null")]
    public void Given_ParameterlessConstructor_When_ExceptionCreated_Then_DefaultMessageAndNullProperties()
    {
        // Act
        var exception = new BadFormatException();

        // Assert
        Assert.NotNull(exception.Message); // Exception sempre tem Message (default do .NET)
        Assert.Null(exception.PropertyName);
        Assert.Null(exception.ExpectedFormat);
        Assert.Null(exception.InnerException);
    }

    [Fact(DisplayName = "Given message constructor, when exception created, then Message is set and properties are null")]
    public void Given_MessageConstructor_When_ExceptionCreated_Then_MessageIsSetAndPropertiesAreNull()
    {
        // Arrange
        const string message = "Custom error message";

        // Act
        var exception = new BadFormatException(message);

        // Assert
        Assert.Equal(message, exception.Message);
        Assert.Null(exception.PropertyName);
        Assert.Null(exception.ExpectedFormat);
        Assert.Null(exception.InnerException);
    }

    [Fact(DisplayName = "Given propertyName and expectedFormat, when exception created, then Message is formatted and properties are set")]
    public void Given_PropertyNameAndExpectedFormat_When_ExceptionCreated_Then_MessageFormattedAndPropertiesSet()
    {
        // Arrange
        const string propertyName = "Email";
        const string expectedFormat = "user@example.com";

        // Act
        var exception = new BadFormatException(propertyName, expectedFormat);

        // Assert
        Assert.Equal(propertyName, exception.PropertyName);
        Assert.Equal(expectedFormat, exception.ExpectedFormat);

        var expectedMessage = $"The property '{propertyName}' has a bad format.{Environment.NewLine}Expected format: {expectedFormat}";
        Assert.Equal(expectedMessage, exception.Message);
        Assert.Null(exception.InnerException);
    }

    [Fact(DisplayName = "Given message and inner exception, when exception created, then Message and InnerException are set")]
    public void Given_MessageAndInnerException_When_ExceptionCreated_Then_MessageAndInnerExceptionSet()
    {
        // Arrange
        const string message = "Outer exception message";
        var innerException = new InvalidOperationException("Inner exception");

        // Act
        var exception = new BadFormatException(message, innerException);

        // Assert
        Assert.Equal(message, exception.Message);
        Assert.Same(innerException, exception.InnerException);
        Assert.Null(exception.PropertyName);
        Assert.Null(exception.ExpectedFormat);
    }

    [Fact(DisplayName = "Given message, expectedFormat and inner exception, when exception created, then Message is formatted and InnerException is set")]
    public void Given_MessageExpectedFormatAndInnerException_When_ExceptionCreated_Then_MessageFormattedAndInnerExceptionSet()
    {
        // Arrange
        const string message = "PhoneNumber";
        const string expectedFormat = "+55 (11) 99999-9999";
        var innerException = new FormatException("Invalid phone format");

        // Act
        var exception = new BadFormatException(message, expectedFormat, innerException);

        // Assert
        Assert.Equal(expectedFormat, exception.ExpectedFormat);
        Assert.Same(innerException, exception.InnerException);

        var expectedMessage = $"The property '{message}' has a bad format.{Environment.NewLine}Expected format: {expectedFormat}";
        Assert.Equal(expectedMessage, exception.Message);

        // PropertyName não é setado neste construtor (só ExpectedFormat)
        Assert.Null(exception.PropertyName);
    }

    [Fact(DisplayName = "Given propertyName and expectedFormat with special characters, when exception created, then Message contains special characters")]
    public void Given_PropertyNameAndExpectedFormatWithSpecialCharacters_When_ExceptionCreated_Then_MessageContainsSpecialCharacters()
    {
        // Arrange
        const string propertyName = "CPF";
        const string expectedFormat = "###.###.###-##";

        // Act
        var exception = new BadFormatException(propertyName, expectedFormat);

        // Assert
        Assert.Contains(propertyName, exception.Message);
        Assert.Contains(expectedFormat, exception.Message);
        Assert.Contains("has a bad format", exception.Message);
    }

    [Fact(DisplayName = "Given exception thrown, when caught, then can be caught as Exception base type")]
    public void Given_ExceptionThrown_When_Caught_Then_CanBeCaughtAsExceptionBaseType()
    {
        // Arrange & Act
        Exception? caughtException;

        try
        {
            throw new BadFormatException("Test", "Expected");
        }
        catch (Exception ex)
        {
            caughtException = ex;
        }

        // Assert
        Assert.NotNull(caughtException);
        Assert.IsType<BadFormatException>(caughtException);
    }
}