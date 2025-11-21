using Ambev.DeveloperEvaluation.Domain.Entities;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class RatingTests
{
    [Fact(DisplayName = "Rating properties should be set and retrieved correctly")]
    public void Given_RatingProperties_When_Set_Then_PropertiesAreCorrect()
    {
        // Arrange
        var rating = new Rating
        {
            // Act
            Rate = 4.5m,
            Count = 100
        };

        // Assert
        Assert.Equal(4.5m, rating.Rate);
        Assert.Equal(100, rating.Count);
    }
}