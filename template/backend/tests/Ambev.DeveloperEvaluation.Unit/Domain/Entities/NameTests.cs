using Ambev.DeveloperEvaluation.Domain.Entities;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class NameTests
{
    [Fact(DisplayName = "Name properties should be set and FullName calculated correctly")]
    public void Given_NameProperties_When_Set_Then_FullNameIsCorrect()
    {
        // Arrange
        var name = new Name
        {
            // Act
            FirstName = "John",
            LastName = "Doe"
        };

        // Assert
        Assert.Equal("John", name.FirstName);
        Assert.Equal("Doe", name.LastName);
        Assert.Equal("John Doe", name.FullName);
    }
}