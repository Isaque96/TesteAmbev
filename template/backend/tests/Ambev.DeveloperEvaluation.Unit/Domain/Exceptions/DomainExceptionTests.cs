using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Exceptions;

public class DomainExceptionTests
{
    [Fact(DisplayName = "Given message, when DomainException created, then message is set correctly")]
    public void Given_Message_When_DomainExceptionCreated_Then_MessageIsSetCorrectly()
    {
        const string message = "message";
        var exception = new DomainException(message);

        Assert.Equal(message, exception.Message);
    }

    [Fact(DisplayName = "Given message and inner exception, when DomainException created, then properties are set correctly")]
    public void Given_MessageAndInnerException_When_DomainExceptionCreated_Then_PropertiesAreSetCorrectly()
    {
        const string message = "message";
        var innerException = new Exception("inner exception");
        var exception = new DomainException(message, innerException);

        Assert.Equal(message, exception.Message);
        Assert.Equal(innerException, exception.InnerException);
    }
}