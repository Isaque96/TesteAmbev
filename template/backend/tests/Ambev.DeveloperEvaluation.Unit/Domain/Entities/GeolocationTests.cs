using Ambev.DeveloperEvaluation.Domain.Entities;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class GeolocationTests
{
    [Fact(DisplayName = "Geolocation properties should be set and retrieved correctly")]
    public void Given_GeolocationProperties_When_Set_Then_PropertiesAreCorrect()
    {
        // Arrange
        var geo = new Geolocation
        {
            // Act
            Lat = "10.12345",
            Long = "-20.54321"
        };

        // Assert
        Assert.Equal("10.12345", geo.Lat);
        Assert.Equal("-20.54321", geo.Long);
    }
}